using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

namespace GBICodeGenPlugin
{
    public abstract class OutputCodeWriter
    {
        protected OutputCodeWriter()
        {

        }

        protected virtual CodeGeneratorOptions Options { get; } = new CodeGeneratorOptions
        {
            VerbatimOrder = true,
            BracingStyle = "C"
        };

        protected virtual CodeDomProvider Provider { get; } = new Microsoft.CSharp.CSharpCodeProvider();

        public abstract void Write(CodeNamespace code);

        public abstract void Write(string name, CodeCompileUnit unit);

        protected void Write(TextWriter writer, CodeCompileUnit compileUnit)
        {
            using (var semicolonWriter = new SemicolonRemovalTextWriter(writer))
            {
                Provider.GenerateCodeFromCompileUnit(compileUnit, semicolonWriter, Options);

            }

        }

        private class SemicolonRemovalTextWriter : TextWriter
        {
            private readonly TextWriter _other;

            private bool _previousWasClosingBrace;

            public SemicolonRemovalTextWriter(TextWriter other)
            {
                _other = other;
            }

            public override Encoding Encoding => _other.Encoding;            

            public override void Write(char value)
            {
                if (!(value == ';' && _previousWasClosingBrace))
                {
                    _other.Write(value);
                }

                _previousWasClosingBrace = value == '}';
            }
        }
        
    }
}
