using System;

namespace BuilderPatternExample
{
    /// <summary>
    /// Product class: Computer with multiple optional attributes
    /// </summary>
    public class Computer
    {
        // Attributes
        public string CPU { get; }
        public string RAM { get; }
        public string Storage { get; }
        public string GPU { get; }
        public int PowerWatts { get; }
        public string OperatingSystem { get; }

        // Private constructor that takes Builder as parameter
        private Computer(Builder builder)
        {
            CPU = builder.CPU;
            RAM = builder.RAM;
            Storage = builder.Storage;
            GPU = builder.GPU;
            PowerWatts = builder.PowerWatts;
            OperatingSystem = builder.OperatingSystem;
        }

        // Static nested Builder class
        public class Builder
        {
            // Builder attributes (same as Computer)
            public string CPU { get; set; }
            public string RAM { get; set; }
            public string Storage { get; set; }
            public string GPU { get; set; }
            public int PowerWatts { get; set; }
            public string OperatingSystem { get; set; }

            // Method to set CPU
            public Builder WithCPU(string cpu)
            {
                CPU = cpu;
                return this;
            }

            // Method to set RAM
            public Builder WithRAM(string ram)
            {
                RAM = ram;
                return this;
            }

            // Method to set Storage
            public Builder WithStorage(string storage)
            {
                Storage = storage;
                return this;
            }

            // Method to set GPU
            public Builder WithGPU(string gpu)
            {
                GPU = gpu;
                return this;
            }

            // Method to set Power
            public Builder WithPower(int powerWatts)
            {
                PowerWatts = powerWatts;
                return this;
            }

            // Method to set Operating System
            public Builder WithOperatingSystem(string os)
            {
                OperatingSystem = os;
                return this;
            }

            // Build method that returns Computer instance
            public Computer Build()
            {
                return new Computer(this);
            }
        }

        // Method to display Computer details
        public void DisplayDetails()
        {
            Console.WriteLine("\n=== Computer Configuration ===");
            Console.WriteLine($"CPU: {CPU ?? "Not specified"}");
            Console.WriteLine($"RAM: {RAM ?? "Not specified"}");
            Console.WriteLine($"Storage: {Storage ?? "Not specified"}");
            Console.WriteLine($"GPU: {GPU ?? "Not specified"}");
            Console.WriteLine($"Power: {PowerWatts}W");
            Console.WriteLine($"Operating System: {OperatingSystem ?? "Not specified"}");
            Console.WriteLine("============================\n");
        }
    }
}