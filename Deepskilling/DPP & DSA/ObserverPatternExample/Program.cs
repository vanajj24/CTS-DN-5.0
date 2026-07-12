using System;

namespace ObserverPatternExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Observer Pattern Demo - Stock Market Monitoring ===\n");

            // Create the stock market (subject)
            StockMarket stockMarket = new StockMarket();

            // Create observers
            Console.WriteLine("1. Creating Observers:");
            MobileApp mobileApp = new MobileApp("user123");
            WebApp webApp = new WebApp("john.doe", "session456");
            EmailNotifier emailNotifier = new EmailNotifier("alerts@example.com");
            AlertSystem alertSystem = new AlertSystem("ALERT-001", 150.00, "above");

            // Register observers
            Console.WriteLine("\n2. Registering Observers:");
            stockMarket.RegisterObserver(mobileApp);
            stockMarket.RegisterObserver(webApp);
            stockMarket.RegisterObserver(emailNotifier);

            Console.WriteLine($"Total observers registered: {stockMarket.GetObserverCount()}\n");

            // Set initial stock prices
            Console.WriteLine("3. Setting Initial Stock Prices:");
            stockMarket.SetStockPrice("AAPL", 145.50);
            stockMarket.SetStockPrice("GOOGL", 2800.00);

            // Update stock prices
            Console.WriteLine("\n4. Updating Stock Prices:");
            stockMarket.SetStockPrice("AAPL", 148.75);

            // Add another observer dynamically
            Console.WriteLine("\n5. Adding New Observer Dynamically:");
            stockMarket.RegisterObserver(alertSystem);
            Console.WriteLine($"Total observers now: {stockMarket.GetObserverCount()}\n");

            // Update stock to trigger alert
            Console.WriteLine("6. Updating Stock to Trigger Alert:");
            stockMarket.SetStockPrice("AAPL", 152.00);

            // Remove an observer
            Console.WriteLine("\n7. Removing Observer:");
            stockMarket.DeregisterObserver(emailNotifier);
            Console.WriteLine($"Total observers now: {stockMarket.GetObserverCount()}\n");

            // Update stock again (email notifier won't receive notification)
            Console.WriteLine("8. Updating Stock After Removing Email Notifier:");
            stockMarket.SetStockPrice("AAPL", 155.25);

            // Disable email notifications and re-register
            Console.WriteLine("\n9. Re-enabling Email Notifier:");
            emailNotifier.EnableNotifications();
            stockMarket.RegisterObserver(emailNotifier);

            // Update multiple stocks
            Console.WriteLine("\n10. Updating Multiple Stocks:");
            stockMarket.SetStockPrice("GOOGL", 2850.00);
            stockMarket.SetStockPrice("AAPL", 158.00);

            // Test alert system with "below" condition
            Console.WriteLine("\n11. Testing Alert System with 'below' Condition:");
            AlertSystem lowAlert = new AlertSystem("ALERT-002", 150.00, "below");
            stockMarket.RegisterObserver(lowAlert);
            stockMarket.SetStockPrice("AAPL", 145.00);

            // Final observer count
            Console.WriteLine("\n12. Final Statistics:");
            Console.WriteLine($"Total registered observers: {stockMarket.GetObserverCount()}");

            Console.WriteLine("\n=== Observer Pattern Demo Complete ===");
            Console.WriteLine("Key Benefits:");
            Console.WriteLine("  Loose Coupling: Subject and observers are loosely coupled");
            Console.WriteLine("  Dynamic Relations: Observers can be added/removed at runtime");
            Console.WriteLine("  Broadcast Updates: Changes automatically propagated to all observers");
            Console.WriteLine("  Open/Closed Principle: Add new observers without modifying subject");
        }
    }
}