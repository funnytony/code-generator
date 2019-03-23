using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBICodeGenPlugin
{
    public class FileOutputCodeWriter : OutputCodeWriter
    {
        public FileOutputCodeWriter(string directory, bool createIfNotExist = true)
        {
            OutputDirectory = directory;
            if (createIfNotExist && !Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }
                
        }

        public string OutputDirectory { get; }

        public override void Write(CodeNamespace code)
        {
            var _compileUnit = new CodeCompileUnit();
            _compileUnit.Namespaces.Add(code);
            Write(Path.Combine(OutputDirectory, code.Name + ".cs"), _compileUnit);
        }

        public override void Write(string fileName, CodeCompileUnit compileUnit)
        {
            try
            {
                using (var _fileStream = new FileStream(fileName, FileMode.Create))
                {
                    using (var _streamWriter = new StreamWriter(_fileStream))
                    {
                        Write(_streamWriter, compileUnit);
                    }
                }
            }catch
            {

            }
            
        }
        
    }
}
