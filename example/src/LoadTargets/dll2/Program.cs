using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using t4_interface;
using Interface;

namespace ddl2
{
    public class Program : IOutputs
    {
        public string Output()
        {
            // 実行例
            var data = new ClassInfo() { ClassName = "test2" };
            ITransformText page = new test1_cs(data);
            var pageContent = page.TransformText();

            if (!Directory.Exists("Generated"))
            {
                Directory.CreateDirectory("Generated");
            }
            File.WriteAllText("Generated/test2.cs", pageContent);

            return $"Generated/test2.csを作成しました。";
        }
    }
}
