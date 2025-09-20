using System.Globalization;
using System.Net.Mime;
using IN12B8_WindowsService.CoreLogic;

namespace IN12B8_WindowsService;
using System.IO.Ports;
using System;

public class BteSerialClient
{
    private SerialPort? _serialPort;
    private bool _connected = false;
    
    public async Task Connect()
    {
        await FindAndConnectPort();
    }

    public void SendString(string msg)
    {
        if (!msg.EndsWith("\n"))
        {
            msg += "\n";
        }

        if (!msg.EndsWith("end.\n"))
        {
            msg = "9999999900end.\n";
        }
        
        if (!_connected)
        {
            return;
        }
        
        try
        {
            _serialPort?.WriteLine(msg);
        }
        catch
        {
            _connected = false;
            _serialPort?.Close();
            Task.Run(FindAndConnectPort);
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

    private async Task FindAndConnectPort()
    {
        string portString = TxtHandler.ReadTxt("comport.txt");
        
        if (!Int32.TryParse(portString, out int port))
        {
            await Task.Delay(10000);
            await FindAndConnectPort();
            return;
        }
        
        CreateSerialPort(port);
        
        for (int i = 0; i < 10; i++)
        {
            CreateSerialPort(port);
            
            if (await TryOpenSerialPort())
            {
                Console.WriteLine("successfully connected to COM" + port);
                _connected = true;
                return;
            }
            
            Console.WriteLine("COM" + port + " returned error");
            await Task.Delay(1500);
        }

        await FindAndConnectPort();
    }

    private async Task<bool> TryOpenSerialPort()
    {
        try
        {
            _serialPort?.Open();
            await Task.Delay(500);
            SendTestMessage();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void SendTestMessage()
    {
        _serialPort?.WriteLine("0123456700end.\n");
    }
}