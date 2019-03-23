using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GBICodeGenPlugin
{
    public class FileModel
    {
        //колекция для используемых библиотек
        private readonly List<string> imports;

        //коллекция для всех пространств имен файла
        private readonly List<NameSpaceModel> namespaces;

        public FileModel(XmlDocument document)
        {
            var _root = document.FirstChild;
            Name = _root.Name;

            imports = new List<string>();

            namespaces = new List<NameSpaceModel>();
            
            if(_root.HasChildNodes)
            {
                var _xdocument = DocumentToXDocument(document);

                //получаем список используемых библиотек
                var _rootelement = _xdocument.Root;
                
                var _imports = from _element in _rootelement.Elements("imports").Elements("import") select _element.Value;
                imports.AddRange(_imports);

                //получаем список пространств имен файла
                var _namespaces = from _element in _rootelement.Elements("namespaces").Elements("namespace") select new NameSpaceModel(_element);
                namespaces.AddRange(_namespaces);
            }
            
        }

        public string Name { get; private set; }

        private XDocument DocumentToXDocument(XmlDocument document) => XDocument.Parse(document.OuterXml);

        public CodeCompileUnit Generate()
        {
            var _codeUnit = new CodeCompileUnit();

            if(imports.Count > 0)
            {
                var _globalNamecpace = new CodeNamespace();
                _globalNamecpace.Imports.AddRange(imports.Select(i=>new CodeNamespaceImport(i)).ToArray());
                _codeUnit.Namespaces.Add(_globalNamecpace);
            }

            if(namespaces.Count > 0)
            {
                _codeUnit.Namespaces.AddRange(namespaces.Select(n => n.Generate()).ToArray());
            }

            return _codeUnit;
        }
    }
}
