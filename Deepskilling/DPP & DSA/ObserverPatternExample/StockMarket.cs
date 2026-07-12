using System;
using System.Collections.Generic;

namespace ObserverPatternExample
{
    public class StockMarket : IStock
    {
        private readonly List<IObserver> _observers;
        private readonly Dictionary<string, double> _stockPrices;

        public StockMarket()
        {
            _observers = new List<IObserver>();
            _stockPrices = new Dictionary<string, double>();
        }

        public void RegisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                Console.WriteLine($"Observer registered: {observer.GetType().Name}");
            }
        }

        public void DeregisterObserver(IObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
                Console.WriteLine($"Observer deregistered: {observer.GetType().Name}");
            }
        }

        public void NotifyObservers()
        {
            Console.WriteLine("\n--- Notifying all observers ---");
            
            foreach (var observer in _observers)
            {
                foreach (var stock in _stockPrices)
                {
                    observer.Update(stock.Key, stock.Value);
                }
            }
            
            Console.WriteLine("--- Notification complete ---\n");
        }

        public void SetStockPrice(string symbol, double price)
        {
            if (_stockPrices.ContainsKey(symbol))
            {
                double oldPrice = _stockPrices[symbol];
                _stockPrices[symbol] = price;
                
                Console.WriteLine($"\nStock Price Updated: {symbol}");
                Console.WriteLine($"  Old Price: ${oldPrice}");
                Console.WriteLine($"  New Price: ${price}");
                Console.WriteLine($"  Change: ${price - oldPrice}");
                
                NotifyObservers();
            }
            else
            {
                _stockPrices[symbol] = price;
                Console.WriteLine($"\nNew Stock Added: {symbol} at ${price}");
                NotifyObservers();
            }
        }

        public double GetStockPrice(string symbol)
        {
            if (_stockPrices.ContainsKey(symbol))
            {
                return _stockPrices[symbol];
            }
            return 0;
        }

        public int GetObserverCount()
        {
            return _observers.Count;
        }
    }
}