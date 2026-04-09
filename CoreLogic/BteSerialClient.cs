using System.IO.Ports;

namespace IN12B8_WindowsService.CoreLogic;

public class BteSerialClient : IDisposable
{
    private SerialPort? _serialPort;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private readonly int _reconnectDelay = 2000;

    public bool Connected => _serialPort?.IsOpen ?? false;
    
    public async Task StartAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if (!Connected)
            {
                await TryConnectAsync();
            }
            
            await Task.Delay(_reconnectDelay, ct);
        }
    }
    
    private async Task TryConnectAsync()
    {
        if (!await _connectionLock.WaitAsync(0)) return;

        try
        {
            string portString = TxtHandler.ReadTxt("comport.txt");
            if (!int.TryParse(portString, out int portNumber)) return;
            ClosePort();
            CreateSerialPort(portNumber);
            if (_serialPort == null) return;
            
            _serialPort.ErrorReceived += OnSerialError;
            _serialPort.Open();
            _serialPort.WriteLine("0123456700end.");

            Console.WriteLine($"Connected to COM{portNumber}");
        }
        catch (Exception ex)
        {
            ClosePort();
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
        finally
        {
            _connectionLock.Release();
        }
    }
    
    public bool SendData(string msg)
    {
        if (!Connected) return false;

        try
        {
            if (!msg.EndsWith("end.\n")) 
            {
                msg = msg.TrimEnd() + "end.\n";
            }

            _serialPort!.WriteLine(msg);
            return true;
        }
        catch
        {
            ClosePort();
            return false;
        }
    }

    private void CreateSerialPort(int number)
    {
        _serialPort = new SerialPort
        {
            PortName = "COM" + number,
            BaudRate = 9600,
            Parity = Parity.None,
            DataBits = 8,
            StopBits = StopBits.One,
            Handshake = Handshake.None,
            ReadTimeout = 500,
            WriteTimeout = 500
        };
    }
    
    private void OnSerialError(object sender, SerialErrorReceivedEventArgs e)
    {
        Console.WriteLine("Serial port error detected!");
        ClosePort();
    }

    private void ClosePort()
    {
        try
        {
            if (_serialPort == null) return;
            _serialPort.ErrorReceived -= OnSerialError;
            if (_serialPort.IsOpen) _serialPort.Close();
            _serialPort.Dispose();
            _serialPort = null;
        }
        catch { /* Ignore cleanup errors */ }
    }

    public void Dispose()
    {
        ClosePort();
        _connectionLock.Dispose();
    }
}