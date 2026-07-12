namespace AdapterPatternExample
{
    /// <summary>
    /// Adaptee 1: PayPal Gateway (has its own specific interface)
    /// </summary>
    public class PayPalGateway
    {
        public string SendPayment(double amount, string currencyCode)
        {
            // PayPal-specific payment processing
            return $"PayPal: Payment of {amount} {currencyCode} processed successfully via PayPal API";
        }

        public bool ValidateAccount(string accountId)
        {
            return !string.IsNullOrEmpty(accountId);
        }
    }

    /// <summary>
    /// Adaptee 2: Stripe Gateway (different interface from PayPal)
    /// </summary>
    public class StripeGateway
    {
        public bool ChargeCard(double totalAmount, string currency, string cardToken)
        {
            // Stripe-specific card charging
            bool success = totalAmount > 0;
            return success;
        }

        public string GetTransactionId()
        {
            return $"STR-{DateTime.Now.Ticks}";
        }
    }

    /// <summary>
    /// Adaptee 3: Razorpay Gateway (Indian payment gateway, different interface)
    /// </summary>
    public class RazorpayGateway
    {
        public int CreatePaymentOrder(double amountInRupees, string currencyType)
        {
            // Razorpay-specific order creation
            // Returns order ID as integer
            return (int)(amountInRupees * 100);
        }

        public string CapturePayment(int orderId)
        {
            return $"Razorpay: Payment captured for Order ID: {orderId}";
        }
    }

    /// <summary>
    /// Adaptee 4: CryptoWallet Gateway (cryptocurrency, completely different interface)
    /// </summary>
    public class CryptoWalletGateway
    {
        public string TransferCrypto(double cryptoAmount, string walletAddress, string cryptoType)
        {
            // Crypto-specific transfer
            return $"CryptoWallet: Transferred {cryptoAmount} {cryptoType} to {walletAddress}";
        }

        public double GetCurrentRate(string cryptoCurrency)
        {
            // Simulated crypto rate
            return cryptoCurrency == "BTC" ? 65000.00 : 3500.00;
        }
    }
}