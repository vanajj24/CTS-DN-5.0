using System;

namespace AdapterPatternExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Adapter Pattern Demo - Payment Processing System ===\n");

            // Client code uses IPaymentProcessor interface uniformly
            // No need to know about different gateway implementations

            // Test 1: PayPal Payment
            Console.WriteLine("1. Processing Payment via PayPal:");
            IPaymentProcessor paypalProcessor = new PayPalAdapter();
            string paypalResult = paypalProcessor.ProcessPayment(150.00, "USD");
            Console.WriteLine(paypalResult);

            // Test 2: Stripe Payment
            Console.WriteLine("\n2. Processing Payment via Stripe:");
            IPaymentProcessor stripeProcessor = new StripeAdapter();
            string stripeResult = stripeProcessor.ProcessPayment(299.99, "USD");
            Console.WriteLine(stripeResult);

            // Test 3: Razorpay Payment (Indian Rupee)
            Console.WriteLine("\n3. Processing Payment via Razorpay:");
            IPaymentProcessor razorpayProcessor = new RazorpayAdapter();
            string razorpayResult = razorpayProcessor.ProcessPayment(5000, "INR");
            Console.WriteLine(razorpayResult);

            // Test 4: Crypto Wallet Payment
            Console.WriteLine("\n4. Processing Payment via CryptoWallet:");
            IPaymentProcessor cryptoProcessor = new CryptoWalletAdapter();
            string cryptoResult = cryptoProcessor.ProcessPayment(1000, "USD");
            Console.WriteLine(cryptoResult);

            // Test 5: EUR Payment via PayPal
            Console.WriteLine("\n5. Processing EUR Payment via PayPal:");
            string eurResult = paypalProcessor.ProcessPayment(75.50, "EUR");
            Console.WriteLine(eurResult);

            // Test 6: Error Case - Negative Amount
            Console.WriteLine("\n6. Error Case - Negative Amount:");
            string errorResult = stripeProcessor.ProcessPayment(-50, "USD");
            Console.WriteLine(errorResult);

            // Demonstrate flexibility: Switch gateways without changing client code
            Console.WriteLine("\n=== Demonstrating Adapter Pattern Flexibility ===");
            Console.WriteLine("Client code remains the same regardless of payment gateway:\n");

            ProcessPaymentThroughGateway("PayPal", new PayPalAdapter(), 500, "USD");
            ProcessPaymentThroughGateway("Stripe", new StripeAdapter(), 500, "USD");
            ProcessPaymentThroughGateway("Razorpay", new RazorpayAdapter(), 500, "INR");

            Console.WriteLine("\n=== Adapter Pattern Demo Complete ===");
            Console.WriteLine("Key Benefit: New payment gateways can be added without modifying client code!");
        }

        /// <summary>
        /// Helper method to demonstrate uniform interface usage
        /// Client code doesn't need to know which gateway is being used
        /// </summary>
        static void ProcessPaymentThroughGateway(string gatewayName, IPaymentProcessor processor, double amount, string currency)
        {
            Console.WriteLine($"{gatewayName}: {processor.ProcessPayment(amount, currency)}");
        }
    }
}