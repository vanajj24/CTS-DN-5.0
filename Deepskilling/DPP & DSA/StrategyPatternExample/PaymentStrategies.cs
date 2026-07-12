using System;

namespace StrategyPatternExample
{
    public class CreditCardPayment : IPaymentStrategy
    {
        private readonly string _cardNumber;
        private readonly string _cardHolderName;
        private readonly string _expiryDate;
        private readonly string _cvv;

        public CreditCardPayment(string cardNumber, string cardHolderName, string expiryDate, string cvv)
        {
            _cardNumber = cardNumber;
            _cardHolderName = cardHolderName;
            _expiryDate = expiryDate;
            _cvv = cvv;
        }

        public bool Pay(double amount)
        {
            if (!ValidatePaymentDetails())
            {
                Console.WriteLine("Payment failed: Invalid credit card details");
                return false;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Payment failed: Amount must be positive");
                return false;
            }

            Console.WriteLine($"Processing Credit Card payment of ${amount}");
            Console.WriteLine($"  Card Holder: {_cardHolderName}");
            Console.WriteLine($"  Card Number: {_cardNumber.Substring(0, 4)}********{_cardNumber.Substring(_cardNumber.Length - 4)}");
            Console.WriteLine($"  Expiry: {_expiryDate}");
            
            // Simulate payment processing
            Console.WriteLine("  Payment processed successfully!");
            return true;
        }

        public string GetPaymentMethod()
        {
            return "Credit Card";
        }

        public bool ValidatePaymentDetails()
        {
            bool isValid = !string.IsNullOrEmpty(_cardNumber) &&
                         !string.IsNullOrEmpty(_cardHolderName) &&
                         !string.IsNullOrEmpty(_expiryDate) &&
                         !string.IsNullOrEmpty(_cvv) &&
                         _cardNumber.Length == 16 &&
                         _cvv.Length == 3;

            return isValid;
        }
    }

    public class PayPalPayment : IPaymentStrategy
    {
        private readonly string _email;
        private readonly string _password;

        public PayPalPayment(string email, string password)
        {
            _email = email;
            _password = password;
        }

        public bool Pay(double amount)
        {
            if (!ValidatePaymentDetails())
            {
                Console.WriteLine("Payment failed: Invalid PayPal credentials");
                return false;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Payment failed: Amount must be positive");
                return false;
            }

            Console.WriteLine($"Processing PayPal payment of ${amount}");
            Console.WriteLine($"  Email: {_email}");
            
            // Simulate PayPal payment processing
            Console.WriteLine("  Logging into PayPal...");
            Console.WriteLine("  Payment processed successfully!");
            return true;
        }

        public string GetPaymentMethod()
        {
            return "PayPal";
        }

        public bool ValidatePaymentDetails()
        {
            bool isValid = !string.IsNullOrEmpty(_email) &&
                         !string.IsNullOrEmpty(_password) &&
                         _email.Contains("@");

            return isValid;
        }
    }

    public class BankTransferPayment : IPaymentStrategy
    {
        private readonly string _accountNumber;
        private readonly string _routingNumber;
        private readonly string _accountHolderName;

        public BankTransferPayment(string accountNumber, string routingNumber, string accountHolderName)
        {
            _accountNumber = accountNumber;
            _routingNumber = routingNumber;
            _accountHolderName = accountHolderName;
        }

        public bool Pay(double amount)
        {
            if (!ValidatePaymentDetails())
            {
                Console.WriteLine("Payment failed: Invalid bank account details");
                return false;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Payment failed: Amount must be positive");
                return false;
            }

            Console.WriteLine($"Processing Bank Transfer payment of ${amount}");
            Console.WriteLine($"  Account Holder: {_accountHolderName}");
            Console.WriteLine($"  Account Number: {_accountNumber.Substring(0, 4)}****");
            Console.WriteLine($"  Routing Number: {_routingNumber}");
            
            // Simulate bank transfer processing
            Console.WriteLine("  Transfer initiated successfully!");
            return true;
        }

        public string GetPaymentMethod()
        {
            return "Bank Transfer";
        }

        public bool ValidatePaymentDetails()
        {
            bool isValid = !string.IsNullOrEmpty(_accountNumber) &&
                         !string.IsNullOrEmpty(_routingNumber) &&
                         !string.IsNullOrEmpty(_accountHolderName) &&
                         _accountNumber.Length >= 8 &&
                         _routingNumber.Length == 9;

            return isValid;
        }
    }

    public class CryptoPayment : IPaymentStrategy
    {
        private readonly string _walletAddress;
        private readonly string _cryptoType;
        private readonly string _privateKey;

        public CryptoPayment(string walletAddress, string cryptoType, string privateKey)
        {
            _walletAddress = walletAddress;
            _cryptoType = cryptoType;
            _privateKey = privateKey;
        }

        public bool Pay(double amount)
        {
            if (!ValidatePaymentDetails())
            {
                Console.WriteLine("Payment failed: Invalid crypto wallet details");
                return false;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Payment failed: Amount must be positive");
                return false;
            }

            Console.WriteLine($"Processing Crypto payment of ${amount}");
            Console.WriteLine($"  Crypto Type: {_cryptoType}");
            Console.WriteLine($"  Wallet Address: {_walletAddress.Substring(0, 6)}...{_walletAddress.Substring(_walletAddress.Length - 4)}");
            
            // Simulate crypto payment processing
            Console.WriteLine("  Connecting to blockchain...");
            Console.WriteLine("  Transaction confirmed!");
            return true;
        }

        public string GetPaymentMethod()
        {
            return "Cryptocurrency";
        }

        public bool ValidatePaymentDetails()
        {
            bool isValid = !string.IsNullOrEmpty(_walletAddress) &&
                         !string.IsNullOrEmpty(_cryptoType) &&
                         !string.IsNullOrEmpty(_privateKey) &&
                         _walletAddress.Length >= 20;

            return isValid;
        }
    }
}