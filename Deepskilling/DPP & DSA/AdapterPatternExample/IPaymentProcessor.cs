namespace AdapterPatternExample
{
    /// <summary>
    /// Target Interface: PaymentProcessor
    /// This is the standard interface that the client will use
    /// </summary>
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Process a payment with amount and currency
        /// </summary>
        /// <param name="amount">Payment amount</param>
        /// <param name="currency">Currency code (USD, EUR, INR, etc.)</param>
        /// <returns>Payment result message</returns>
        string ProcessPayment(double amount, string currency);
    }
}