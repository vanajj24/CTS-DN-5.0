namespace AdapterPatternExample
{
    /// <summary>
    /// Adapter 1: PayPal Adapter
    /// Translates IPaymentProcessor interface to PayPalGateway-specific methods
    /// </summary>
    public class PayPalAdapter : IPaymentProcessor
    {
        private readonly PayPalGateway _paypalGateway;

        public PayPalAdapter()
        {
            _paypalGateway = new PayPalGateway();
        }

        public string ProcessPayment(double amount, string currency)
        {
            // Translate standard interface to PayPal-specific method
            if (amount <= 0)
            {
                return "Error: Payment amount must be positive";
            }

            return _paypalGateway.SendPayment(amount, currency);
        }
    }

    /// <summary>
    /// Adapter 2: Stripe Adapter
    /// Translates IPaymentProcessor interface to StripeGateway-specific methods
    /// </summary>
    public class StripeAdapter : IPaymentProcessor
    {
        private readonly StripeGateway _stripeGateway;

        public StripeAdapter()
        {
            _stripeGateway = new StripeGateway();
        }

        public string ProcessPayment(double amount, string currency)
        {
            // Translate standard interface to Stripe-specific method
            if (amount <= 0)
            {
                return "Error: Payment amount must be positive";
            }

            // Simulate card token (in real scenario, this would come from frontend)
            string cardToken = "tok_simulated";

            bool success = _stripeGateway.ChargeCard(amount, currency, cardToken);

            if (success)
            {
                string transactionId = _stripeGateway.GetTransactionId();
                return $"Stripe: Payment of {amount} {currency} processed successfully. Transaction ID: {transactionId}";
            }
            else
            {
                return $"Error: Stripe payment failed for {amount} {currency}";
            }
        }
    }

    /// <summary>
    /// Adapter 3: Razorpay Adapter
    /// Translates IPaymentProcessor interface to RazorpayGateway-specific methods
    /// </summary>
    public class RazorpayAdapter : IPaymentProcessor
    {
        private readonly RazorpayGateway _razorpayGateway;

        public RazorpayAdapter()
        {
            _razorpayGateway = new RazorpayGateway();
        }

        public string ProcessPayment(double amount, string currency)
        {
            // Translate standard interface to Razorpay-specific methods
            if (amount <= 0)
            {
                return "Error: Payment amount must be positive";
            }

            // Razorpay expects INR, convert if needed
            double amountInRupees = currency == "INR" ? amount : amount * 83.50; // Simulated exchange rate

            int orderId = _razorpayGateway.CreatePaymentOrder(amountInRupees, currency);
            string result = _razorpayGateway.CapturePayment(orderId);

            return $"{result} (Amount: {amount} {currency}, Order ID: {orderId})";
        }
    }

    /// <summary>
    /// Adapter 4: CryptoWallet Adapter
    /// Translates IPaymentProcessor interface to CryptoWalletGateway-specific methods
    /// </summary>
    public class CryptoWalletAdapter : IPaymentProcessor
    {
        private readonly CryptoWalletGateway _cryptoGateway;

        public CryptoWalletAdapter()
        {
            _cryptoGateway = new CryptoWalletGateway();
        }

        public string ProcessPayment(double amount, string currency)
        {
            // Translate standard interface to Crypto-specific methods
            if (amount <= 0)
            {
                return "Error: Payment amount must be positive";
            }

            // Assume currency is USD, convert to BTC for crypto payment
            double btcAmount = amount / _cryptoGateway.GetCurrentRate("BTC");
            string walletAddress = "0x742d35Cc6634C0532925a3b844Bc9e7595f0bEb";

            return _cryptoGateway.TransferCrypto(btcAmount, walletAddress, "BTC");
        }
    }
}