using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace GBICodeGenPlugin
{
    public class MemberModel
    {
        private readonly XElement element;
        private readonly string type;
        public MemberModel(XElement element)
        {
            this.element = element;
            if (element.HasElements)
            {
                Name = element.Element("name")?.Value;
                type = element.Element("type")?.Value;
            }
                
        }

        public string Name { get; private set; }

        public void AddMemeber(CodeTypeDeclaration codeType)
        {
            CodeTypeMember _member = new CodeTypeMember();
            
            if (element.Name.ToString().Equals("field"))
            {                
                _member = new CodeMemberField(type, Name);
            }
            else if(element.Name.ToString().Equals("property"))
            {
                _member = new CodeMemberProperty();
                _member.Name = Name;
                (_member as CodeMemberProperty).Type = new CodeTypeReference(type);
                var _getexpressions = element.Element("getexpressions")?.Elements("expression");
                if(_getexpressions != null)
                {
                    foreach (var _expression in _getexpressions)
                    {
                        var _getMethod = new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), _expression.Value));
                        (_member as CodeMemberProperty).GetStatements.Add(_getMethod);
                    }
                }            

                var _setexpressions = element.Element("setexpressions")?.Elements("expression");
                if(_setexpressions != null)
                {
                    foreach (var _expression in _setexpressions)
                    {
                        var _setMethod = new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), _expression.Value), new CodePropertySetValueReferenceExpression());
                        (_member as CodeMemberProperty).SetStatements.Add(_setMethod);
                    }
                }
                
            }


            AddAtributes(_member);
            codeType.Members.Add(_member);
        }

        private void AddAtributes(CodeTypeMember typeMember)
        {
            
            var _isVirtual = false;
            foreach (var _atribute in element.Element("atributes")?.Elements("atribute"))
            {
                switch (_atribute.Value)
                {
                    case "public":
                        typeMember.Attributes = (typeMember.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
                        break;
                    case "protected":
                        typeMember.Attributes = (typeMember.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Family;
                        break;
                    case "internal":
                        typeMember.Attributes = (typeMember.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Assembly;
                        break;
                    case "private":
                        typeMember.Attributes = (typeMember.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Private;
                        break;
                    case "abstract":
                        typeMember.Attributes = (typeMember.Attributes & ~MemberAttributes.ScopeMask) | MemberAttributes.Abstract;
                        break;                    
                    case "static":
                        typeMember.Attributes = (typeMember.Attributes & ~MemberAttributes.ScopeMask) | MemberAttributes.Static;
                        break;
                    case "overide":
                        typeMember.Attributes = (typeMember.Attributes & ~MemberAttributes.ScopeMask) | MemberAttributes.Override;
                        break;
                    case "virtual":
                        _isVirtual = true;
                        break;
                    case "constant":
                        typeMember.Attributes = (typeMember.Attributes & ~MemberAttributes.ScopeMask) | MemberAttributes.Const;
                        break;                    
                }
            }
            if(!_isVirtual)
            {
                typeMember.Attributes = (typeMember.Attributes & ~MemberAttributes.ScopeMask & ~MemberAttributes.AccessMask) | MemberAttributes.Final;
            }


        }
    }
}
