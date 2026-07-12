using System;

namespace CommandPatternExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Command Pattern Demo - Home Automation System ===\n");

            // Create receivers (devices)
            Console.WriteLine("1. Creating Devices:");
            Light livingRoomLight = new Light("Living Room");
            Light kitchenLight = new Light("Kitchen");
            Television bedroomTV = new Television("Bedroom");
            Thermostat officeThermostat = new Thermostat("Office");

            // Create remote control
            Console.WriteLine("\n2. Creating Remote Control:");
            RemoteControl remote = new RemoteControl();

            // Create commands and assign to remote
            Console.WriteLine("\n3. Creating and Assigning Commands:");
            ICommand livingRoomLightOn = new LightOnCommand(livingRoomLight);
            ICommand livingRoomLightOff = new LightOffCommand(livingRoomLight);
            
            remote.SetCommand(livingRoomLightOn);
            Console.WriteLine("Living Room Light ON command assigned");

            // Test 1: Turn on living room light
            Console.WriteLine("\n4. Test 1 - Turn On Living Room Light:");
            remote.PressButton();

            // Test 2: Turn off living room light
            Console.WriteLine("\n5. Test 2 - Turn Off Living Room Light:");
            remote.SetCommand(livingRoomLightOff);
            remote.PressButton();

            // Test 3: Turn on kitchen light using registered button
            Console.WriteLine("\n6. Test 3 - Turn On Kitchen Light (Registered Button):");
            ICommand kitchenLightOn = new LightOnCommand(kitchenLight);
            remote.RegisterButton("Kitchen Light ON", kitchenLightOn);
            remote.PressButtonByName("Kitchen Light ON");

            // Test 4: Turn on TV
            Console.WriteLine("\n7. Test 4 - Turn On Bedroom TV:");
            ICommand tvOn = new TelevisionOnCommand(bedroomTV);
            remote.RegisterButton("TV ON", tvOn);
            remote.PressButtonByName("TV ON");

            // Test 5: Change TV channel
            Console.WriteLine("\n8. Test 5 - Change TV Channel:");
            ICommand changeChannel = new TelevisionChannelCommand(bedroomTV, "Sports");
            remote.RegisterButton("TV Channel Sports", changeChannel);
            remote.PressButtonByName("TV Channel Sports");

            // Test 6: Increase TV volume
            Console.WriteLine("\n9. Test 6 - Increase TV Volume:");
            ICommand volumeUp = new TelevisionVolumeUpCommand(bedroomTV);
            remote.RegisterButton("TV Volume Up", volumeUp);
            remote.PressButtonByName("TV Volume Up");
            remote.PressButtonByName("TV Volume Up");

            // Test 7: Turn on thermostat
            Console.WriteLine("\n10. Test 7 - Turn On Office Thermostat:");
            ICommand thermostatOn = new ThermostatOnCommand(officeThermostat);
            remote.RegisterButton("Thermostat ON", thermostatOn);
            remote.PressButtonByName("Thermostat ON");

            // Test 8: Set thermostat temperature
            Console.WriteLine("\n11. Test 8 - Set Thermostat Temperature:");
            ICommand setTemp = new ThermostatTemperatureCommand(officeThermostat, 75.0);
            remote.RegisterButton("Set Temp 75F", setTemp);
            remote.PressButtonByName("Set Temp 75F");

            // Test 9: Show command history
            Console.WriteLine("\n12. Test 9 - Show Command History:");
            remote.ShowCommandHistory();

            // Test 10: Undo functionality
            Console.WriteLine("\n13. Test 10 - Undo Functionality:");
            Console.WriteLine($"Commands in undo stack: {remote.GetUndoStackSize()}");
            remote.PressUndoButton();

            // Test 11: Multiple devices
            Console.WriteLine("\n14. Test 11 - Multiple Devices:");
            remote.SetCommand(livingRoomLightOn);
            remote.PressButton();
            
            remote.SetCommand(tvOn);
            remote.PressButton();
            
            remote.SetCommand(thermostatOn);
            remote.PressButton();

            // Test 12: Turn off all devices
            Console.WriteLine("\n15. Test 12 - Turn Off All Devices:");
            ICommand tvOff = new TelevisionOffCommand(bedroomTV);
            remote.SetCommand(tvOff);
            remote.PressButton();

            ICommand thermostatOff = new ThermostatOffCommand(officeThermostat);
            remote.SetCommand(thermostatOff);
            remote.PressButton();

            Console.WriteLine("\n=== Command Pattern Demo Complete ===");
            Console.WriteLine("Key Benefits:");
            Console.WriteLine("  Encapsulates Requests: Commands encapsulate action and receiver");
            Console.WriteLine("  Decouples Invoker and Receiver: Remote doesn't know about devices");
            Console.WriteLine("  Easy to Extend: Add new commands without modifying existing code");
            Console.WriteLine("  Supports Undo: Command history enables undo operations");
            Console.WriteLine("  Macro Commands: Can combine multiple commands into one");
        }
    }
}