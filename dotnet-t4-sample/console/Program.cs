using System;
using System.Text;
using System.Text.RegularExpressions;

namespace t4_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new ClassInfo() { ClassName = "test" };
            ITransformText page = new test1_cs(data);
            var pageContent = page.TransformText();

            Console.WriteLine(pageContent);
        }

    }
}
