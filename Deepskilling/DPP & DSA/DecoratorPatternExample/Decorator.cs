using System;

namespace DecoratorPatternExample
{
    /// <summary>
    /// Abstract Decorator: NotifierDecorator
    /// Holds a reference to a Notifier and can be extended by concrete decorators
    /// </summary>
    public abstract class NotifierDecorator : INotifier
    {
        protected readonly INotifier _wrappedNotifier;

        public NotifierDecorator(INotifier notifier)
        {
            _wrappedNotifier = notifier;
        }

        public virtual void Send(string message)
        {
            // Delegate to the wrapped notifier
            _wrappedNotifier.Send(message);
        }
    }

    /// <summary>
    /// Concrete Decorator: SMSNotifierDecorator
    /// Adds SMS notification channel to any existing notifier
    /// </summary>
    public class SMSNotifierDecorator : NotifierDecorator
    {
        private readonly string _phoneNumber;

        public SMSNotifierDecorator(INotifier notifier, string phoneNumber)
            : base(notifier)
        {
            _phoneNumber = phoneNumber;
        }

        public override void Send(string message)
        {
            // First send via original notifier
            _wrappedNotifier.Send(message);

            // Then add SMS notification
            Console.WriteLine($" SMS sent to {_phoneNumber}: {message}");
        }
    }

    /// <summary>
    /// Concrete Decorator: SlackNotifierDecorator
    /// Adds Slack notification channel to any existing notifier
    /// </summary>
    public class SlackNotifierDecorator : NotifierDecorator
    {
        private readonly string _channelName;

        public SlackNotifierDecorator(INotifier notifier, string channelName)
            : base(notifier)
        {
            _channelName = channelName;
        }

        public override void Send(string message)
        {
            // First send via original notifier
            _wrappedNotifier.Send(message);

            // Then add Slack notification
            Console.WriteLine($" Slack message to {_channelName}: {message}");
        }
    }

    /// <summary>
    /// Concrete Decorator: PriorityDecorator
    /// Adds priority level to notifications
    /// </summary>
    public class PriorityDecorator : NotifierDecorator
    {
        private readonly string _priority;

        public PriorityDecorator(INotifier notifier, string priority)
            : base(notifier)
        {
            _priority = priority;
        }

        public override void Send(string message)
        {
            // Add priority information to the message
            string priorityMessage = $"[{_priority}] {message}";
            _wrappedNotifier.Send(priorityMessage);
        }
    }

    /// <summary>
    /// Concrete Decorator: LoggingDecorator
    /// Adds logging functionality to notifications
    /// </summary>
    public class LoggingDecorator : NotifierDecorator
    {
        private readonly string _logFilePath;

        public LoggingDecorator(INotifier notifier, string logFilePath = "notifications.log")
            : base(notifier)
        {
            _logFilePath = logFilePath;
        }

        public override void Send(string message)
        {
            // Log the notification before sending
            LogNotification(message);

            // Then send via wrapped notifier
            _wrappedNotifier.Send(message);
        }

        private void LogNotification(string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($" Logging: [{timestamp}] {message}");
        }
    }

    /// <summary>
    /// Concrete Decorator: EncryptionDecorator
    /// Adds encryption functionality to notifications
    /// </summary>
    public class EncryptionDecorator : NotifierDecorator
    {
        public EncryptionDecorator(INotifier notifier)
            : base(notifier)
        {
        }

        public override void Send(string message)
        {
            // Encrypt the message before sending
            string encryptedMessage = EncryptMessage(message);
            _wrappedNotifier.Send(encryptedMessage);
        }

        private string EncryptMessage(string message)
        {
            // Simulated encryption (in real scenario, use proper encryption)
            return $" ENCRYPTED: {message} ";
        }
    }
}