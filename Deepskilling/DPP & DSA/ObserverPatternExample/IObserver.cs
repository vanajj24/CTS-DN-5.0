namespace ObserverPatternExample
{
    public interface IObserver
    {
        void Update(string symbol, double price);
    }
}