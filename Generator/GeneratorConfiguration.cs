using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GBICodeGenPlugin
{
    public class GeneratorConfiguration
    {
        //регулярное выражение для идентификаторов
        //public static Regex IdentifireRegx { get; } = new Regex(@"^@?[_\p{L}\p{Nl}][\p{L}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*$", RegexOptions.Compiled);

        //класс для создания выходных файлов с кодом
        public OutputCodeWriter CodeWriter { get; set; }
        //папка для хранения выходного файла
        public string OutputFolder { get; set; }
    }
}
