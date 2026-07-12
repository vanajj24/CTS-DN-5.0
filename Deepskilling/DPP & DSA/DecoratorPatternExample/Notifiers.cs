using System;

namespace DecoratorPatternExample
{
    /// <summary>
    /// Concrete Component: SMSNotifier
    /// Basic notification implementation via SMS
    /// </summary>
    public class SMSNotifier : INotifier
    {
        private readonly string _phoneNumber;

        public SMSNotifier(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
        }

        public void Send(string message)
        {
            Console.WriteLine($" SMS sent to {_phoneNumber}: {message}");
        }
    }

    /// <summary>
    /// Concrete Component: SlackNotifier
    /// Basic notification implementation via Slack
    /// </summary>
    public class SlackNotifier : INotifier
    {
        private readonly string _channelName;

        public SlackNotifier(string channelName)
        {
            _channelName = channelName;
        }

        public void Send(string message)
        {
            Console.WriteLine($" Slack message to {_channelName}: {message}");
        }
    }

    /// <summary>
    /// Concrete Component: WhatsAppNotifier
    /// Basic notification implementation via WhatsApp
    /// </summary>
    public class WhatsAppNotifier : INotifier
    {
        private readonly string _whatsappId;

        public WhatsAppNotifier(string whatsappId)
        {
            _whatsappId = whatsappId;
        }

        public void Send(string message)
        {
            Console.WriteLine($"📲 WhatsApp sent to {_whatsappId}: {message}");
        }
    }
}