namespace ProxyPatternExample
{
    /// <summary>
    /// Subject Interface: Image
    /// This defines the standard interface for both RealImage and ProxyImage
    /// </summary>
    public interface IImage
    {
        /// <summary>
        /// Display the image
        /// </summary>
        void Display();

        /// <summary>
        /// Get image filename
        /// </summary>
        string GetFileName();

        /// <summary>
        /// Get image size in bytes
        /// </summary>
        long GetFileSize();
    }
}