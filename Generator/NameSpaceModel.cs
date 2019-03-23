using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GBICodeGenPlugin
{
    public class NameSpaceModel
    {
        private readonly List<TypeModel> types;

        public NameSpaceModel(XElement element)
        {
            Name = element.Attribute("name").Value;

            types = new List<TypeModel>();

            if(element.HasElements)
            {
                var _types = from _element in element.Elements("types").Elements("type") select _element;
                foreach(var _type in _types)
                {
                    if(_type.HasAttributes)
                    {
                        var _typeAttribute = _type.Attribute("type")?.Value;
                        if(_typeAttribute.Equals("class"))
                        {
                            types.Add(new ClassModel(_type));
                        }
                    }
                }
            }

        }

        public string Name { get; private set; }

        public CodeNamespace Generate()
        {
            var _namespace = new CodeNamespace(Name);
            
            _namespace.Types.AddRange(types.Select(t => t.Generate()).ToArray());

            return _namespace;
        }
    }
}
