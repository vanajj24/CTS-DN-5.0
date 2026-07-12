using System;

namespace FactoryMethodPatternExample.Documents
{
    public class PdfDocument : IDocument
    {
        public void Open()
        {
            Console.WriteLine("PDF Document Opened");
        }
    }
}