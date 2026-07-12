using System;

namespace DecoratorPatternExample
{
    /// <summary>
    /// Concrete Component: EmailNotifier
    /// Basic notification implementation via Email
    /// </summary>
    public class EmailNotifier : INotifier
    {
        private readonly string _emailAddress;

        public EmailNotifier(string emailAddress)
        {
            _emailAddress = emailAddress;
        }

        public void Send(string message)
        {
            Console.WriteLine($" Email sent to {_emailAddress}: {message}");
        }
    }
}