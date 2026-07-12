using System;

namespace FactoryMethodPatternExample.Documents
{
    public class ExcelDocument : IDocument
    {
        public void Open()
        {
            Console.WriteLine("Excel Document Opened");
        }
    }
}