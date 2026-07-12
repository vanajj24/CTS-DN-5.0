using System;
using System.Threading;

namespace ProxyPatternExample
{
    public class RealImage : IImage
    {
        private readonly string _fileName;
        private readonly string _serverUrl;
        private long _fileSize;
        private byte[] _imageData;
        private bool _isLoaded;

        public RealImage(string fileName, string serverUrl = "https://images.example.com")
        {
            _fileName = fileName;
            _serverUrl = serverUrl;
            _isLoaded = false;
            _fileSize = 0;
            _imageData = Array.Empty<byte>();
        }

        public void LoadFromServer()
        {
            if (_isLoaded)
            {
                Console.WriteLine($"  Image {_fileName} already loaded");
                return;
            }

            Console.WriteLine($"  Loading image {_fileName} from {_serverUrl}...");

            Thread.Sleep(2000);

            _imageData = GenerateSimulatedImageData(_fileName);
            _fileSize = _imageData.Length;
            _isLoaded = true;

            Console.WriteLine($"  Image {_fileName} loaded successfully");
            Console.WriteLine($"  File size: {FormatFileSize(_fileSize)}");
        }

        public void Display()
        {
            LoadFromServer();

            Console.WriteLine($"  Displaying image: {_fileName}");
            Console.WriteLine($"  Dimensions: {GetImageDimensions()}");
            Console.WriteLine($"  Image displayed with {_imageData.Length / 1000} KB of data");
        }

        public string GetFileName()
        {
            return _fileName;
        }

        public long GetFileSize()
        {
            return _fileSize;
        }

        private byte[] GenerateSimulatedImageData(string fileName)
        {
            int sizeMultiplier = fileName.Contains("large") ? 5000 : 
                               fileName.Contains("medium") ? 2000 : 1000;
            
            byte[] imageData = new byte[sizeMultiplier];
            
            for (int i = 0; i < imageData.Length; i++)
            {
                imageData[i] = (byte)(i % 256);
            }

            return imageData;
        }

        private string GetImageDimensions()
        {
            if (_fileName.Contains("large"))
                return "1920x1080 pixels";
            else if (_fileName.Contains("medium"))
                return "1280x720 pixels";
            else
                return "800x600 pixels";
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }

        public bool IsLoaded()
        {
            return _isLoaded;
        }
    }
}