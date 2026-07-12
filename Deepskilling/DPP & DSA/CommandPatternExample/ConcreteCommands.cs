using System;

namespace CommandPatternExample
{
    public class LightOnCommand : ICommand
    {
        private readonly Light _light;

        public LightOnCommand(Light light)
        {
            _light = light;
        }

        public void Execute()
        {
            _light.TurnOn();
        }
    }

    public class LightOffCommand : ICommand
    {
        private readonly Light _light;

        public LightOffCommand(Light light)
        {
            _light = light;
        }

        public void Execute()
        {
            _light.TurnOff();
        }
    }

    public class TelevisionOnCommand : ICommand
    {
        private readonly Television _television;

        public TelevisionOnCommand(Television television)
        {
            _television = television;
        }

        public void Execute()
        {
            _television.TurnOn();
        }
    }

    public class TelevisionOffCommand : ICommand
    {
        private readonly Television _television;

        public TelevisionOffCommand(Television television)
        {
            _television = television;
        }

        public void Execute()
        {
            _television.TurnOff();
        }
    }

    public class TelevisionChannelCommand : ICommand
    {
        private readonly Television _television;
        private readonly string _channel;

        public TelevisionChannelCommand(Television television, string channel)
        {
            _television = television;
            _channel = channel;
        }

        public void Execute()
        {
            _television.SetChannel(_channel);
        }
    }

    public class TelevisionVolumeUpCommand : ICommand
    {
        private readonly Television _television;

        public TelevisionVolumeUpCommand(Television television)
        {
            _television = television;
        }

        public void Execute()
        {
            _television.IncreaseVolume();
        }
    }

    public class TelevisionVolumeDownCommand : ICommand
    {
        private readonly Television _television;

        public TelevisionVolumeDownCommand(Television television)
        {
            _television = television;
        }

        public void Execute()
        {
            _television.DecreaseVolume();
        }
    }

    public class ThermostatOnCommand : ICommand
    {
        private readonly Thermostat _thermostat;

        public ThermostatOnCommand(Thermostat thermostat)
        {
            _thermostat = thermostat;
        }

        public void Execute()
        {
            _thermostat.TurnOn();
        }
    }

    public class ThermostatOffCommand : ICommand
    {
        private readonly Thermostat _thermostat;

        public ThermostatOffCommand(Thermostat thermostat)
        {
            _thermostat = thermostat;
        }

        public void Execute()
        {
            _thermostat.TurnOff();
        }
    }

    public class ThermostatTemperatureCommand : ICommand
    {
        private readonly Thermostat _thermostat;
        private readonly double _temperature;

        public ThermostatTemperatureCommand(Thermostat thermostat, double temperature)
        {
            _thermostat = thermostat;
            _temperature = temperature;
        }

        public void Execute()
        {
            _thermostat.SetTemperature(_temperature);
        }
    }
}