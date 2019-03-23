using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace GBICodeGenPlugin
{
    public class ClassModel : TypeModel
    {
        private readonly List<string> baseTypes;

        private readonly List<string> atributes;

        private readonly List<XElement> members;

        public ClassModel(XElement element) : base(element)
        {
            if(element.HasElements)
            {
                baseTypes = new List<string>();
                var _baseTypes = from _element in element.Elements("basetypes").Elements("basetype") select _element.Value;
                baseTypes.AddRange(_baseTypes);

                atributes = new List<string>();
                var _atributes = from _element in element.Elements("atributes").Elements("atribute") select _element.Value;
                atributes.AddRange(_atributes);

                members = new List<XElement>();
                var _members = from _elment in element.Elements("members").Elements() select _elment;
                members.AddRange(_members);


            }
        }

        public override CodeTypeDeclaration Generate()
        {
            var _clasDeclaration =  base.Generate();
            _clasDeclaration.IsClass = true;

            if (baseTypes.Count > 0)
                AddBaseTypes(_clasDeclaration);

            if (atributes.Count > 0)
                AddAtributes(_clasDeclaration);

            if (members.Count > 0)
                AddMemebers(_clasDeclaration);

            return _clasDeclaration;
        }

        private void AddMemebers(CodeTypeDeclaration clasDeclaration)
        {
            foreach(var _element in members)
            {
                new MemberModel(_element).AddMemeber(clasDeclaration);
            }
        }

        private void AddAtributes(CodeTypeDeclaration clasDeclaration)
        {
            var _isVirtual = false;
            if (atributes.Contains("partial"))
                clasDeclaration.IsPartial = true;            
            foreach(var atribute in atributes)
            {
                switch (atribute)
                {
                    case "public":
                        clasDeclaration.TypeAttributes = (clasDeclaration.TypeAttributes & ~TypeAttributes.LayoutMask & ~TypeAttributes.VisibilityMask) | TypeAttributes.Public;
                        break;
                    case "protected":
                        clasDeclaration.Attributes = (clasDeclaration.Attributes & ~MemberAttributes.AccessMask & ~MemberAttributes.ScopeMask) | MemberAttributes.Family;
                        break;
                    case "internal":
                        clasDeclaration.TypeAttributes = (clasDeclaration.TypeAttributes & ~TypeAttributes.LayoutMask & ~TypeAttributes.VisibilityMask) | TypeAttributes.NotPublic;
                        break;
                    case "private":
                        clasDeclaration.Attributes = (clasDeclaration.Attributes & ~MemberAttributes.AccessMask & ~MemberAttributes.ScopeMask) | MemberAttributes.Private;
                        break;
                    case "abstract":
                        clasDeclaration.TypeAttributes = clasDeclaration.TypeAttributes | TypeAttributes.Abstract;
                        break;
                    case "seald":
                        clasDeclaration.TypeAttributes = (clasDeclaration.TypeAttributes & ~TypeAttributes.LayoutMask & ~TypeAttributes.VisibilityMask) | TypeAttributes.Sealed;
                        break;
                    case "static":
                        clasDeclaration.Attributes = (clasDeclaration.Attributes & ~MemberAttributes.AccessMask & ~MemberAttributes.ScopeMask) | MemberAttributes.Static;
                        break;
                    case "virtual":
                        _isVirtual = true;
                        break;
                }
            }

            //if (!_isVirtual)
            //{
            //    clasDeclaration.Attributes = (clasDeclaration.Attributes & ~MemberAttributes.ScopeMask & ~MemberAttributes.AccessMask) | MemberAttributes.Final;
            //}


        }

        private void AddBaseTypes(CodeTypeDeclaration clasDeclaration)
        {
            clasDeclaration.BaseTypes.AddRange(baseTypes.Select(t => new CodeTypeReference(t)).ToArray());
        }
    }
}
