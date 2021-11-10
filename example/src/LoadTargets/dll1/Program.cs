using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using t4_interface;
using Interface;

namespace t4_practice
{
    public class Program : IOutputs
    {
        public string Output()
        {
            // 実行例
            var data = new ClassInfo() { ClassName = "test" };
            ITransformText page = new test1_cs(data);
            var pageContent = page.TransformText();

            if (!Directory.Exists("Generated"))
            {
                Directory.CreateDirectory("Generated");
            }
            File.WriteAllText("Generated/test.cs", pageContent);

            return $"Generated/test.csを作成しました。";
        }
    }
}
