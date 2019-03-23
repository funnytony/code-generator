using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace GBICodeGenPlugin
{
    public partial class Generator
    {
        //класс настроек
        private readonly GeneratorConfiguration configuration = new GeneratorConfiguration();

        //путь к папке где будут создаваться новые cs файлы
        public String OutPutFolder
        {
            get=>configuration.OutputFolder;
            set=>configuration.OutputFolder = value;
        }

        //класс формирующий выходной файл
        public OutputCodeWriter CodeWriter
        {
            get => configuration.CodeWriter; 
            set => configuration.CodeWriter = value; 
        }
        //основная функция генерирующая код, в котороую передается список xml файлов 
        public void Generate(IEnumerable<string> files)
        {
            //читаем xml ффайлы
            
            var _documents = files.Select((f)=> {
                var _document = new XmlDocument();
                _document.Load(f);
                return _document;
            });
            var _model = new ModelBuilder(configuration, _documents);
            var _outputFiles = _model.Generate();
            foreach(var _outputFile in _outputFiles)
            {
                CodeWriter.Write(Path.Combine(OutPutFolder, _outputFile.Key+".cs"), _outputFile.Value);
            }
        }

    }
}
