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
            System.IO.File.WriteAllText($"{nameof(test1_cs)}.TransformText.txt", ReadT4("test1_cs", nameof(t4_practice)));

            Console.WriteLine(pageContent);
            Console.WriteLine();
            Console.WriteLine(ReadT4("test1_cs", nameof(t4_practice)));
        }

        static string ReadT4(string templateName, string nameSpace)
        {
            var templateFile = RemoveParams(System.IO.File.ReadAllText($"{templateName}.tt"));
            return CreateTransformText(templateFile);

            string RemoveParams(string src)
            {
                src = GetRegex(src, $"<#@ template (.+?) #>{Environment.NewLine}", string.Empty);
                src = GetRegex(src, $"<#@ assembly (.+?) #>{Environment.NewLine}", string.Empty);
                src = GetRegex(src, $"<#@ output (.+?) #>{Environment.NewLine}", string.Empty);
                return src;
            }

            // using
            string ConvertUsings(string src)
            {
                return GetRegex(src, "<#@ import namespace=\"(.+?)\" #>", "using $1;");
            }

            // params
            string ConvertParams(string src)
            {
                return GetRegex(src, "<#= (.+?) #>", "{$1}");
            }

            // 最終出力
            string CreateTransformText(string src)
            {
                var result = new StringBuilder();
                result.AppendLine("using System.Text;");
                result.AppendLine($"namespace {nameSpace}");
                result.AppendLine("{");
                result.AppendLine($"  public partial class {templateName}");
                result.AppendLine("   {");
                result.AppendLine("     public string TransformText()");
                result.AppendLine("     {");

                result.AppendLine("       var template = new StringBuilder();");

                var rgx = new Regex("({|})");
                var templates = ConvertParams(ConvertUsings(templateFile)).Split(Environment.NewLine);
                foreach (var line in templates)
                {
                    var outputLine = line;
                    if(rgx.Matches(outputLine).Count < 2)
                    {
                        outputLine = outputLine.Replace("{","{{").Replace("}","}}");
                    }
                    result.AppendLine($"       template.AppendLine($\"{outputLine}\");");
                }

                result.AppendLine("       return template.ToString();");
                result.AppendLine("     }");
                result.AppendLine("   }");
                result.AppendLine("}");

                return result.ToString();
            }

            string GetRegex(string src, string reg, string replace)
            {
                return Regex.Replace(src, reg, replace);
            }
        }
    }
}
