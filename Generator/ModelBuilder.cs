using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GBICodeGenPlugin
{
    internal class ModelBuilder
    {
        //класс настроек
        private readonly GeneratorConfiguration configuration;

        private readonly List<XmlDocument> documents;

        public ModelBuilder(GeneratorConfiguration configuration, IEnumerable<XmlDocument> documents)
        {
            this.configuration = configuration;
            this.documents = new List<XmlDocument>(documents);

        }

        public Dictionary<string, CodeCompileUnit> Generate()
        {
            return documents.Select(d => new FileModel(d)).ToDictionary(fm => fm.Name, fm => fm.Generate());
        }
    }
}
