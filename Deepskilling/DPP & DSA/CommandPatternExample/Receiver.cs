using System;

namespace CommandPatternExample
{
    public class Light
    {
        private readonly string _location;
        private bool _isOn;

        public Light(string location)
        {
            _location = location;
            _isOn = false;
        }

        public void TurnOn()
        {
            _isOn = true;
            Console.WriteLine($"Light in {_location} is now ON");
        }

        public void TurnOff()
        {
            _isOn = false;
            Console.WriteLine($"Light in {_location} is now OFF");
        }

        public bool IsOn()
        {
            return _isOn;
        }

        public string GetLocation()
        {
            return _location;
        }
    }

    public class Television
    {
        private readonly string _location;
        private bool _isOn;
        private int _volume;
        private string _channel;

        public Television(string location)
        {
            _location = location;
            _isOn = false;
            _volume = 0;
            _channel = "None";
        }

        public void TurnOn()
        {
            _isOn = true;
            _volume = 10;
            _channel = "News";
            Console.WriteLine($"Television in {_location} is now ON");
            Console.WriteLine($"  Volume: {_volume}, Channel: {_channel}");
        }

        public void TurnOff()
        {
            _isOn = false;
            _volume = 0;
            Console.WriteLine($"Television in {_location} is now OFF");
        }

        public void SetChannel(string channel)
        {
            if (_isOn)
            {
                _channel = channel;
                Console.WriteLine($"Television channel changed to: {_channel}");
            }
        }

        public void IncreaseVolume()
        {
            if (_isOn && _volume < 100)
            {
                _volume++;
                Console.WriteLine($"Television volume increased to: {_volume}");
            }
        }

        public void DecreaseVolume()
        {
            if (_isOn && _volume > 0)
            {
                _volume--;
                Console.WriteLine($"Television volume decreased to: {_volume}");
            }
        }

        public bool IsOn()
        {
            return _isOn;
        }

        public string GetLocation()
        {
            return _location;
        }
    }

    public class Thermostat
    {
        private readonly string _location;
        private double _temperature;
        private bool _isOn;

        public Thermostat(string location)
        {
            _location = location;
            _isOn = false;
            _temperature = 70.0;
        }

        public void TurnOn()
        {
            _isOn = true;
            Console.WriteLine($"Thermostat in {_location} is now ON");
            Console.WriteLine($"  Current Temperature: {_temperature}F");
        }

        public void TurnOff()
        {
            _isOn = false;
            Console.WriteLine($"Thermostat in {_location} is now OFF");
        }

        public void SetTemperature(double temperature)
        {
            if (_isOn)
            {
                _temperature = temperature;
                Console.WriteLine($"Thermostat temperature set to: {_temperature}F");
            }
        }

        public double GetTemperature()
        {
            return _temperature;
        }

        public bool IsOn()
        {
            return _isOn;
        }

        public string GetLocation()
        {
            return _location;
        }
    }
}