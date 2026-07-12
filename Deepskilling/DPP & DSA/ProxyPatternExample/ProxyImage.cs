using System;
using System.Collections.Generic;

namespace ProxyPatternExample
{
    public class ProxyImage : IImage
    {
        private readonly string _fileName;
        private readonly string _serverUrl;
        private RealImage? _realImage;
        private bool _isLoaded;

        private static readonly Dictionary<string, RealImage> _imageCache = new Dictionary<string, RealImage>();
        private static int _proxyCreationCount = 0;

        public ProxyImage(string fileName, string serverUrl = "https://images.example.com")
        {
            _fileName = fileName;
            _serverUrl = serverUrl;
            _realImage = null;
            _isLoaded = false;
            
            _proxyCreationCount++;
            Console.WriteLine($"  Proxy created for {_fileName} (Proxy #{_proxyCreationCount})");
        }

        public void Display()
        {
            if (_realImage == null)
            {
                Console.WriteLine($"  Lazy loading: {_fileName} not loaded yet");
                
                if (_imageCache.ContainsKey(_fileName))
                {
                    Console.WriteLine($"  Cache hit: Loading {_fileName} from cache");
                    _realImage = _imageCache[_fileName];
                    _isLoaded = true;
                }
                else
                {
                    Console.WriteLine($"  Cache miss: Loading {_fileName} from server");
                    _realImage = new RealImage(_fileName, _serverUrl);
                    _imageCache[_fileName] = _realImage;
                }
            }

            _realImage.Display();
            _isLoaded = true;
        }

        public string GetFileName()
        {
            return _fileName;
        }

        public long GetFileSize()
        {
            if (_realImage == null)
            {
                return 0;
            }
            return _realImage.GetFileSize();
        }

        public bool IsLoaded()
        {
            return _isLoaded;
        }

        public static int GetCacheSize()
        {
            return _imageCache.Count;
        }

        public static void ClearCache()
        {
            _imageCache.Clear();
            Console.WriteLine("  Image cache cleared");
        }
    }
}