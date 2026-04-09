using System.Globalization;
using IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects;
using IN12B8_WindowsService.Providers.FancyDivergenceMeter.Effects.Changers;

namespace IN12B8_WindowsService.Providers.FancyDivergenceMeter;

public class FancyDivergenceMeterProvider(int duration)
    : FormattedStringProvider(duration)
{
    private readonly MoeDivergenceApiClient _api = new();
    private readonly List<DivergenceMeterEffect> _effects = [
        new UnstableDigitEffect(12, new TubeCyclicChanger()),
        new FailingTubeEffect(),
        new SequentialStabilizeEffect(4, new TubeCyclicChanger()),
        new SequentialFailEffect(new RandomChanger()),
    ];

    private readonly DivergenceMeterEffect _transitionEffect =
        new SequentialStabilizeEffect(30, new ExcludingRandomChanger());

    private readonly float _randomEffectChance = 0.5f;
    
    private readonly Random _random = new();
    private bool _isRunning;
    private double _currentDivergence;
    private int[] _displayValue = [0,0,0,0,0,0,0];
    private DivergenceMeterEffect? _activeEffect;

    public override void Init()
    {
        _isRunning = true;
        Task.Run(ApiCallLoop);
    }

    public override string GetValueString()
    {
        ProcessEffect();
        string number = "";
        foreach (int t in _displayValue)
        {
            number += t >= 0 ? t.ToString() : "-";
        }
        return $"{number.Insert(1, "-")}40end.\n";
    }

    private void ProcessEffect()
    {
        if (_activeEffect != null)
        {
            _displayValue = _activeEffect.GetNextFrameDigits();
            if (_activeEffect.IsFinished) _activeEffect = null;
        }
        else
        {
            _displayValue = Decompose(_currentDivergence);
        }
    }

    private async Task ApiCallLoop()
    {
        while (_isRunning)
        {
            double newValue = await _api.GetDivergence();

            if (Math.Abs(newValue - _currentDivergence) > 0.000001)
            {
                _currentDivergence = newValue;
                TriggerTransitionEffect(newValue);
            }
            else if (_random.NextSingle() < _randomEffectChance && (_activeEffect?.IsFinished ?? true))
            {
                TriggerRandomEffect(newValue);
            }
            
            await Task.Delay(3000);
        }
    }
    
    private void TriggerRandomEffect(double targetValue)
    {
        if (_effects.Count == 0) return;
        _activeEffect = _effects[_random.Next(_effects.Count)];
        _activeEffect.Reset(targetValue);
    }

    private void TriggerTransitionEffect(double targetValue)
    {
        _activeEffect = _transitionEffect;
        _activeEffect.Reset(targetValue);
    }
    
    private int[] Decompose(double val) => 
        val.ToString("F6", CultureInfo.InvariantCulture).Replace(".", "").PadRight(7, '0').Select(c => c - '0').ToArray();
}