namespace DecoratorPatternExample
{
    /// <summary>
    /// Component Interface: Notifier
    /// This defines the standard interface for all notifiers
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// Send a notification with the given message
        /// </summary>
        /// <param name="message">The notification message</param>
        void Send(string message);
    }
}