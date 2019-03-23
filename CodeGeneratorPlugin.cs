using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;
using GBICodeGenPlugin;

namespace Geekbrains
{
    public class CodeGeneratorPlugin
    {
        [MenuItem("Tools/Code/Generate")]
        private static void GenerateCode()
        {
            
            var _generator = new Generator();

            _generator.OutPutFolder = Path.Combine(Application.dataPath, @"Scripts/Generated");
            _generator.CodeWriter = new FileOutputCodeWriter(_generator.OutPutFolder);
            var _xmlFiles = Directory.EnumerateFiles(Path.Combine(Application.dataPath, @"XML/Codeconfigs"),"*.xml");
            _generator.Generate(_xmlFiles);
        }
    }
}
