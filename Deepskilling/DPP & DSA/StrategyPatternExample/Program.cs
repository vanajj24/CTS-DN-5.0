using System;

namespace StrategyPatternExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Strategy Pattern Demo - Payment System ===\n");

            // Create payment context
            PaymentContext paymentContext = new PaymentContext();

            // Test 1: Credit Card Payment
            Console.WriteLine("1. Credit Card Payment:");
            IPaymentStrategy creditCard = new CreditCardPayment(
                "1234567890123456",
                "John Doe",
                "12/25",
                "123"
            );
            paymentContext.SetPaymentStrategy(creditCard);
            paymentContext.ExecutePayment(150.00);

            // Test 2: PayPal Payment
            Console.WriteLine("\n2. PayPal Payment:");
            IPaymentStrategy paypal = new PayPalPayment(
                "john.doe@example.com",
                "securePassword123"
            );
            paymentContext.SetPaymentStrategy(paypal);
            paymentContext.ExecutePayment(299.99);

            // Test 3: Bank Transfer Payment
            Console.WriteLine("\n3. Bank Transfer Payment:");
            IPaymentStrategy bankTransfer = new BankTransferPayment(
                "9876543210",
                "123456789",
                "John Doe"
            );
            paymentContext.SetPaymentStrategy(bankTransfer);
            paymentContext.ExecutePayment(5000.00);

            // Test 4: Crypto Payment
            Console.WriteLine("\n4. Crypto Payment:");
            IPaymentStrategy crypto = new CryptoPayment(
                "0x742d35Cc6634C0532925a3b844Bc9e7595f0bEb1",
                "BTC",
                "cryptoPrivateKey123"
            );
            paymentContext.SetPaymentStrategy(crypto);
            paymentContext.ExecutePayment(1000.00);

            // Test 5: Change payment strategy dynamically
            Console.WriteLine("\n5. Changing Payment Strategy Dynamically:");
            Console.WriteLine("Switching from Crypto to Credit Card:");
            IPaymentStrategy newCreditCard = new CreditCardPayment(
                "9876543210987654",
                "Jane Smith",
                "06/26",
                "456"
            );
            paymentContext.SetPaymentStrategy(newCreditCard);
            paymentContext.ExecutePayment(75.50);

            // Test 6: Invalid payment details
            Console.WriteLine("\n6. Payment with Invalid Details:");
            IPaymentStrategy invalidCard = new CreditCardPayment(
                "123",
                "",
                "",
                ""
            );
            paymentContext.SetPaymentStrategy(invalidCard);
            paymentContext.ExecutePayment(100.00);

            // Test 7: Invalid amount
            Console.WriteLine("\n7. Payment with Invalid Amount:");
            IPaymentStrategy validCard = new CreditCardPayment(
                "1234567890123456",
                "John Doe",
                "12/25",
                "123"
            );
            paymentContext.SetPaymentStrategy(validCard);
            paymentContext.ExecutePayment(-50.00);

            // Test 8: No payment strategy selected
            Console.WriteLine("\n8. Payment Without Strategy:");
            PaymentContext newContext = new PaymentContext();
            newContext.ExecutePayment(100.00);

            // Test 9: Multiple payment strategies in sequence
            Console.WriteLine("\n9. Multiple Payment Strategies in Sequence:");
            PaymentContext orderContext = new PaymentContext();

            Console.WriteLine("Order 1 - Credit Card:");
            orderContext.SetPaymentStrategy(new CreditCardPayment(
                "1111222233334444",
                "Alice Brown",
                "03/27",
                "789"
            ));
            orderContext.ExecutePayment(250.00);

            Console.WriteLine("Order 2 - PayPal:");
            orderContext.SetPaymentStrategy(new PayPalPayment(
                "alice@example.com",
                "password456"
            ));
            orderContext.ExecutePayment(180.00);

            Console.WriteLine("Order 3 - Bank Transfer:");
            orderContext.SetPaymentStrategy(new BankTransferPayment(
                "5555666677",
                "987654321",
                "Alice Brown"
            ));
            orderContext.ExecutePayment(10000.00);

            // Final statistics
            Console.WriteLine("\n10. Final Statistics:");
            var currentStrategy = paymentContext.GetCurrentStrategy();
            Console.WriteLine($"Current Payment Method: {currentStrategy?.GetPaymentMethod() ?? "None"}");
            Console.WriteLine($"Last Payment Amount: ${paymentContext.GetTotalAmount()}");
            Console.WriteLine($"Last Payment Date: {paymentContext.GetPaymentDate()}");

            Console.WriteLine("\n=== Strategy Pattern Demo Complete ===");
            Console.WriteLine("Key Benefits:");
            Console.WriteLine("  Encapsulates Algorithms: Each payment method is encapsulated in its own class");
            Console.WriteLine("  Runtime Selection: Payment strategy can be selected at runtime");
            Console.WriteLine("  Easy to Extend: Add new payment methods without modifying existing code");
            Console.WriteLine("  Separation of Concerns: Payment logic separated from payment context");
        }
    }
}