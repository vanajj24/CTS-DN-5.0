using System;

namespace BuilderPatternExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Builder Pattern Demo - Computer Configurations ===\n");

            // Configuration 1: High-end Gaming Computer
            Console.WriteLine("1. Creating High-End Gaming Computer:");
            var gamingComputer = new Computer.Builder()
                .WithCPU("Intel Core i9-14900K")
                .WithRAM("32GB DDR5")
                .WithStorage("2TB NVMe SSD")
                .WithGPU("NVIDIA RTX 4090")
                .WithPower(850)
                .WithOperatingSystem("Windows 11 Pro")
                .Build();

            gamingComputer.DisplayDetails();

            // Configuration 2: Budget Office Computer
            Console.WriteLine("2. Creating Budget Office Computer:");
            var officeComputer = new Computer.Builder()
                .WithCPU("Intel Core i5-13400")
                .WithRAM("16GB DDR4")
                .WithStorage("500GB SSD")
                .WithPower(450)
                .WithOperatingSystem("Windows 11 Home")
                .Build();

            officeComputer.DisplayDetails();

            // Configuration 3: Minimal Computer (only essential parts)
            Console.WriteLine("3. Creating Minimal Computer:");
            var minimalComputer = new Computer.Builder()
                .WithCPU("AMD Ryzen 5 7600")
                .WithRAM("8GB DDR5")
                .Build();

            minimalComputer.DisplayDetails();

            // Configuration 4: Professional Workstation
            Console.WriteLine("4. Creating Professional Workstation:");
            var workstation = new Computer.Builder()
                .WithCPU("AMD Ryzen 9 7950X")
                .WithRAM("64GB DDR5")
                .WithStorage("4TB NVMe SSD + 8TB HDD")
                .WithGPU("NVIDIA RTX 4080")
                .WithPower(1000)
                .WithOperatingSystem("Ubuntu 24.04")
                .Build();

            workstation.DisplayDetails();

            Console.WriteLine("=== Builder Pattern Demo Complete ===");
        }
    }
}