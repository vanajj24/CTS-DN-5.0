using System;

namespace DecoratorPatternExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Decorator Pattern Demo - Notification System ===\n");

            // Test 1: Basic Email Notification (no decorator)
            Console.WriteLine("1. Basic Email Notification:");
            INotifier emailNotifier = new EmailNotifier("user@example.com");
            emailNotifier.Send("Your order has been shipped!");

            // Test 2: Email + SMS Notification (single decorator)
            Console.WriteLine("\n2. Email + SMS Notification (Single Decorator):");
            INotifier emailWithSMS = new SMSNotifierDecorator(
                new EmailNotifier("user@example.com"),
                "+1-234-567-8900"
            );
            emailWithSMS.Send("Your order has been shipped!");

            // Test 3: Email + SMS + Slack (multiple decorators)
            Console.WriteLine("\n3. Email + SMS + Slack (Multiple Decorators):");
            INotifier multiChannel = new SlackNotifierDecorator(
                new SMSNotifierDecorator(
                    new EmailNotifier("user@example.com"),
                    "+1-234-567-8900"
                ),
                "notifications-team"
            );
            multiChannel.Send("Server alert: High CPU usage detected!");

            // Test 4: Email with Priority + Logging
            Console.WriteLine("\n4. Email with Priority + Logging:");
            INotifier priorityLogged = new LoggingDecorator(
                new PriorityDecorator(
                    new EmailNotifier("admin@example.com"),
                    "URGENT"
                )
            );
            priorityLogged.Send("Database connection failed!");

            // Test 5: SMS with Encryption
            Console.WriteLine("\n5. SMS with Encryption:");
            INotifier encryptedSMS = new EncryptionDecorator(
                new SMSNotifier("+1-987-654-3210")
            );
            encryptedSMS.Send("Your account password is: secret123");

            // Test 6: Complex Notification Chain (5 decorators)
            Console.WriteLine("\n6. Complex Notification Chain (5 Decorators):");
            INotifier complexNotification = new LoggingDecorator(
                new EncryptionDecorator(
                    new SlackNotifierDecorator(
                        new SMSNotifierDecorator(
                            new EmailNotifier("critical@example.com"),
                            "+1-555-123-4567"
                        ),
                        "critical-alerts"
                    )
                )
            );
            complexNotification.Send("SYSTEM EMERGENCY: Data center power failure!");

            // Test 7: WhatsApp + Priority
            Console.WriteLine("\n7. WhatsApp + Priority:");
            INotifier whatsappWithPriority = new PriorityDecorator(
                new WhatsAppNotifier("+1-555-999-8888"),
                "INFO"
            );
            whatsappWithPriority.Send("Meeting scheduled for 3 PM tomorrow");

            // Demonstrate flexibility: Stack decorators dynamically
            Console.WriteLine("\n=== Demonstrating Dynamic Decorator Stacking ===");
            Console.WriteLine("Base notifier: Email\n");

            INotifier notifier = new EmailNotifier("test@example.com");
            notifier.Send("Base notification");

            Console.WriteLine("\nAdding SMS decorator:");
            notifier = new SMSNotifierDecorator(notifier, "+1-222-333-4444");
            notifier.Send("With SMS");

            Console.WriteLine("\nAdding Slack decorator:");
            notifier = new SlackNotifierDecorator(notifier, "dev-team");
            notifier.Send("With Slack");

            Console.WriteLine("\nAdding Priority decorator:");
            notifier = new PriorityDecorator(notifier, "HIGH");
            notifier.Send("With Priority");

            Console.WriteLine("\nAdding Logging decorator:");
            notifier = new LoggingDecorator(notifier);
            notifier.Send("With Logging");

            Console.WriteLine("\n=== Decorator Pattern Demo Complete ===");
            Console.WriteLine("Key Benefit: Add functionalities dynamically without modifying existing code!");
        }
    }
}