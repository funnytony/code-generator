using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace GBICodeGenPlugin
{
    public class TypeModel
    {        
            
        public TypeModel(XElement element)
        {
            if (element.HasAttributes)
                Name = element.Attribute("name").Value;
            
            
        }

        public string Name { get; private set; }

        public virtual CodeTypeDeclaration Generate()
        {
            var _type = new CodeTypeDeclaration(Name);

            return _type;
        }
    }
}
