using System;

namespace ProxyPatternExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Proxy Pattern Demo - Image Viewer Application ===\n");

            Console.WriteLine("1. First Load - Cache Miss (Lazy Initialization):");
            IImage image1 = new ProxyImage("image_small.jpg");
            image1.Display();

            Console.WriteLine("\n2. Second Load - Cache Hit (Same Image):");
            IImage image2 = new ProxyImage("image_small.jpg");
            image2.Display();

            Console.WriteLine("\n3. Loading Different Images:");
            IImage image3 = new ProxyImage("image_medium.jpg");
            image3.Display();

            IImage image4 = new ProxyImage("image_large.jpg");
            image4.Display();

            Console.WriteLine("\n4. Loading Previously Cached Image:");
            IImage image5 = new ProxyImage("image_medium.jpg");
            image5.Display();

            Console.WriteLine("\n5. Cache Statistics:");
            Console.WriteLine($"  Cache Size: {ProxyImage.GetCacheSize()} images");

            Console.WriteLine("\n6. Loading Same Image Multiple Times (Caching Benefit):");
            Console.WriteLine("  Loading 'image_small.jpg' 3 more times:");
            
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"\n  Attempt {i}:");
                IImage image = new ProxyImage("image_small.jpg");
                image.Display();
            }

            Console.WriteLine("\n7. Clear Cache and Load Again:");
            ProxyImage.ClearCache();
            
            Console.WriteLine("\n  After clearing cache:");
            IImage image6 = new ProxyImage("image_small.jpg");
            image6.Display();

            Console.WriteLine("\n8. Lazy Initialization Benefit:");
            Console.WriteLine("  Creating 5 proxy objects (no actual loading yet):");
            
            IImage[] images = new IImage[5];
            for (int i = 0; i < 5; i++)
            {
                images[i] = new ProxyImage($"image_{i}.jpg");
            }
            
            Console.WriteLine("  All 5 proxies created instantly (no network delay)");
            Console.WriteLine("  Now displaying first image (triggers actual loading):");
            images[0].Display();
            Console.WriteLine("  Other 4 proxies still not loaded (lazy initialization)");

            Console.WriteLine("\n=== Proxy Pattern Demo Complete ===");
            Console.WriteLine("Key Benefits:");
            Console.WriteLine("  Lazy Initialization: RealImage loaded only when needed");
            Console.WriteLine("  Caching: Images cached to avoid repeated server loads");
            Console.WriteLine("  Performance: Significant speed improvement for repeated access");
            Console.WriteLine("  Transparency: Client uses same interface for proxy and real object");
        }
    }
}
