namespace ObserverPatternExample
{
    public interface IStock
    {
        void RegisterObserver(IObserver observer);
        void DeregisterObserver(IObserver observer);
        void NotifyObservers();
        void SetStockPrice(string symbol, double price);
    }
}