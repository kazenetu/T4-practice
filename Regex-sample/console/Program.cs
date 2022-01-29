using System;
using System.Text;
using System.Text.RegularExpressions;

namespace t4_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            // 実行例
            var data = new ClassInfo() { ClassName = "test" };
            ITransformText page = new test1_cs(data);
            var pageContent = page.TransformText();

            Console.WriteLine($"[test1_cs]");
            Console.WriteLine(pageContent);

            var data2 = new ClassInfo2() { ClassName = "test2" };
            ITransformText page2 = new test2_cs(data2);
            pageContent = page2.TransformText();
            Console.WriteLine($"[test2_cs]");
            Console.WriteLine(pageContent);
        }
    }
}
