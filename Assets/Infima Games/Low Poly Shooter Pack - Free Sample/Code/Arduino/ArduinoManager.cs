
using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoInputReader : MonoBehaviour
{
    [Header("Serial Settings")]
    public string portName = "COM3";
    public int baudRate = 9600;

    private SerialPort serial;

    [Header("Joystick")]
    public int joystickX;
    public int joystickY;

    public float horizontal;
    public float vertical;

    [Header("Buttons")]
    public bool menu;
    public bool shoot;
    public bool reload;

    [Header("Gyroscope")]
    public int gyroX;
    public int gyroY;
    public int gyroZ;

    [Header("Connection")]
    public bool dataReceived;
    public bool isConnected;

    private void Start()
    {
        serial = new SerialPort(portName, baudRate);
        serial.ReadTimeout = 20;

        try
        {
            serial.Open();
            serial.ReadTimeout = 20;

            isConnected = true;

            Debug.Log("Arduino connected: " + portName);
        }
        catch (Exception e)
        {
            isConnected = false;
            Debug.LogError("Arduino connection error: " + e.Message);
        }
    }

    private void Update()
    {
        if (serial == null || !serial.IsOpen)
            return;

        try
        {
            string data = serial.ReadLine();

            string[] values = data.Split(',');

            if (values.Length != 8)
                return;

            // Joystick
            joystickX = int.Parse(values[0]);
            joystickY = int.Parse(values[1]);

            // Normalize joystick (0-1023 to -1 to 1)
            horizontal = (joystickX - 512) / 512f;
            vertical = (joystickY - 512) / 512f;

            // Deadzone
            if (Mathf.Abs(horizontal) < 0.1f)
                horizontal = 0f;

            if (Mathf.Abs(vertical) < 0.1f)
                vertical = 0f;

            // Buttons
            menu = values[2] == "1";
            shoot = values[3] == "1";
            reload = values[4] == "1";

            // Gyroscope analog values
            gyroX = int.Parse(values[5]);
            gyroY = int.Parse(values[6]);
            gyroZ = int.Parse(values[7]);

            dataReceived = true;
        }
        catch (TimeoutException)
        {
            // No new data yet
        }
        catch (FormatException)
        {
            Debug.LogWarning("Invalid Arduino data format");
        }
        catch (Exception e)
        {
            Debug.LogWarning("Arduino read error: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        CloseConnection();
    }

    private void OnDestroy()
    {
        CloseConnection();
    }

    private void CloseConnection()
    {
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
            Debug.Log("Arduino disconnected");
        }
    }
}
