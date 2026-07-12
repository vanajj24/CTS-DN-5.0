namespace StrategyPatternExample
{
    public interface IPaymentStrategy
    {
        bool Pay(double amount);
        string GetPaymentMethod();
        bool ValidatePaymentDetails();
    }
}