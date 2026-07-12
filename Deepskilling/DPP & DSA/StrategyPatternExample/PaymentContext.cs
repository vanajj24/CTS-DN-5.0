using System;

namespace StrategyPatternExample
{
    public class PaymentContext
    {
        private IPaymentStrategy? _paymentStrategy;
        private double _totalAmount;
        private DateTime _paymentDate;

        public PaymentContext()
        {
            _totalAmount = 0;
            _paymentDate = DateTime.MinValue;
            _paymentStrategy = null;
        }

        public void SetPaymentStrategy(IPaymentStrategy strategy)
        {
            _paymentStrategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
            Console.WriteLine($"Payment strategy set to: {_paymentStrategy.GetPaymentMethod()}");
        }

        public bool ExecutePayment(double amount)
        {
            if (_paymentStrategy == null)
            {
                Console.WriteLine("Payment failed: No payment strategy selected");
                return false;
            }

            Console.WriteLine("\n=== Payment Processing ===");
            Console.WriteLine($"Amount: ${amount}");
            Console.WriteLine($"Method: {_paymentStrategy.GetPaymentMethod()}");

            _totalAmount = amount;
            _paymentDate = DateTime.Now;

            bool paymentSuccess = _paymentStrategy.Pay(amount);

            if (paymentSuccess)
            {
                Console.WriteLine($"Payment completed on {_paymentDate}");
                Console.WriteLine($"Total Amount Paid: ${_totalAmount}");
            }

            Console.WriteLine("=== Payment Complete ===\n");

            return paymentSuccess;
        }

        public IPaymentStrategy? GetCurrentStrategy()
        {
            return _paymentStrategy;
        }

        public double GetTotalAmount()
        {
            return _totalAmount;
        }

        public DateTime GetPaymentDate()
        {
            return _paymentDate;
        }
    }
}