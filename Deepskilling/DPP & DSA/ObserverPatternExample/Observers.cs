using System;

namespace ObserverPatternExample
{
    public class MobileApp : IObserver
    {
        private readonly string _appName;
        private readonly string _userId;

        public MobileApp(string userId)
        {
            _appName = "Mobile App";
            _userId = userId;
        }

        public void Update(string symbol, double price)
        {
            Console.WriteLine($"[Mobile App - User {_userId}] Stock Alert: {symbol} = ${price}");
        }

        public string GetAppName()
        {
            return _appName;
        }
    }

    public class WebApp : IObserver
    {
        private readonly string _userName;
        private readonly string _sessionId;

        public WebApp(string userName, string sessionId)
        {
            _userName = userName;
            _sessionId = sessionId;
        }

        public void Update(string symbol, double price)
        {
            Console.WriteLine($"[Web App - User {_userName}] Stock Update: {symbol} = ${price} (Session: {_sessionId})");
        }

        public string GetUserName()
        {
            return _userName;
        }
    }

    public class EmailNotifier : IObserver
    {
        private readonly string _emailAddress;
        private bool _notificationsEnabled;

        public EmailNotifier(string emailAddress)
        {
            _emailAddress = emailAddress;
            _notificationsEnabled = true;
        }

        public void Update(string symbol, double price)
        {
            if (_notificationsEnabled)
            {
                string phoneNumber = _emailAddress;
                Console.WriteLine($"[Email to {_emailAddress}] Sending notification: {symbol} price is now ${price}");
            }
        }

        public void EnableNotifications()
        {
            _notificationsEnabled = true;
            Console.WriteLine($"Email notifications enabled for {_emailAddress}");
        }

        public void DisableNotifications()
        {
            _notificationsEnabled = false;
            Console.WriteLine($"Email notifications disabled for {_emailAddress}");
        }
    }

    public class AlertSystem : IObserver
    {
        private readonly string _alertId;
        private readonly double _thresholdPrice;
        private readonly string _condition;

        public AlertSystem(string alertId, double thresholdPrice, string condition = "above")
        {
            _alertId = alertId;
            _thresholdPrice = thresholdPrice;
            _condition = condition;
        }

        public void Update(string symbol, double price)
        {
            bool shouldAlert = false;

            if (_condition == "above" && price >= _thresholdPrice)
            {
                shouldAlert = true;
            }
            else if (_condition == "below" && price <= _thresholdPrice)
            {
                shouldAlert = true;
            }
            else if (_condition == "exact" && price == _thresholdPrice)
            {
                shouldAlert = true;
            }

            if (shouldAlert)
            {
                Console.WriteLine($"[Alert System {_alertId}] TRIGGERED: {symbol} = ${price}");
                Console.WriteLine($"  Condition: {_condition} ${_thresholdPrice}");
                Console.WriteLine($"  ALERT SENT!");
            }
        }

        public string GetAlertId()
        {
            return _alertId;
        }
    }
}