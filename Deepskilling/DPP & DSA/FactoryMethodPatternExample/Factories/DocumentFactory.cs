using FactoryMethodPatternExample.Documents;

namespace FactoryMethodPatternExample.Factories
{
    public abstract class DocumentFactory
    {
        public abstract IDocument CreateDocument();
    }
}