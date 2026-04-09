using IN12B8_WindowsService.Providers.Helpers;
using LibreHardwareMonitor.Hardware;

namespace IN12B8_WindowsService.Providers;

public class HardwareMonitoringProvider(int duration) : FormattedStringProvider(duration)
{
    private readonly Computer _computer = new Computer()
    {
        IsCpuEnabled = true,
        IsGpuEnabled = true
    };

    private IHardware? _cpu;
    private IHardware? _gpu;
    private ISensor? _cpuTemp;
    private ISensor? _cpuLoad;
    private ISensor? _gpuTemp;
    private ISensor? _gpuLoad;
    
    public override void Init()
    {
        _computer.Open();

        foreach (IHardware hardware in _computer.Hardware)
        {
            if (hardware.HardwareType == HardwareType.Cpu)
            {
                _cpu = hardware;
                
                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (string.Equals(sensor.Name, "CCD1 (Tdie)") && sensor.SensorType == SensorType.Temperature)
                    {
                        _cpuTemp = sensor;
                    }

                    if (string.Equals(sensor.Name, "CPU Total") && sensor.SensorType == SensorType.Load)
                    {
                        _cpuLoad = sensor;
                    }
                }
            }

            if (hardware.HardwareType == HardwareType.GpuNvidia)
            {
                _gpu = hardware;
                
                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (string.Equals(sensor.Name, "GPU Core") && sensor.SensorType == SensorType.Temperature)
                    {
                        _gpuTemp = sensor;
                    }

                    if (string.Equals(sensor.Name, "GPU Core") && sensor.SensorType == SensorType.Load)
                    {
                        _gpuLoad = sensor;
                    }
                }
            }
        }
    }

    public override string GetValueString()
    {
        _cpu?.Update();
        _gpu?.Update();
        
        return "-" + FloatFormatter.FormatFloat3Digits(_cpuTemp?.Value) + "-" +
               FloatFormatter.FormatFloat3Digits(_gpuTemp?.Value) + "00end.\n";
    }
}