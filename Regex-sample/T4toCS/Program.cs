using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace t4_practice
{
  class Program
  {
    public enum Type
    {
      None,
      CodeStart,
      CodeEnd,
      String,
    }

    static void Main(string[] args)
    {
      if (args.Length < 4)
      {
        Console.WriteLine("dotnet T4toCS T4FilePath NameSpaceName ClassName OutputPath");
        return;
      }
      var t4FilePath = args[0];
      var nameSpaceName = args[1];
      var className = args[2];
      var outputPath = args[3];

      // T4テンプレートからC#ソースコードを取得
      var t4String = T4toClassMethod(t4FilePath, nameSpaceName, className);

      // TransformTextメソッドのソースファイルを作成
      if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
      }
      File.WriteAllText(outputPath, t4String);

      // コンソール出力
      Console.WriteLine($"created {outputPath}");
    }

    /// <summary>
    /// T4ファイルをC#ソースコードに変換
    /// </summary>
    /// <param name="templateName">T4ファイル名</param>
    /// <param name="nameSpaceName">namespace名</param>
    /// <param name="className">クラス名</param>
    /// <returns>TransformTextメソッドの文字列</returns>
    static string T4toClassMethod(string templateName, string nameSpaceName, string className)
    {
      // T4ファイルを取得、変換して文字列として返す
      var templateFile = RemoveParams(File.ReadAllText(templateName).Replace("\r", string.Empty));
      return CreateTransformText(templateFile);

      // 不要な設定を削除
      string RemoveParams(string src)
      {
        src = GetRegex(src, $"<#@ template (.+?) #>{'\n'}", string.Empty);
        src = GetRegex(src, $"<#@ assembly (.+?) #>{'\n'}", string.Empty);
        src = GetRegex(src, $"<#@ output (.+?) #>{'\n'}", string.Empty);
        return src;
      }

      // 最終出力
      string CreateTransformText(string src)
      {
        // 該当クラスのTransformTextメソッドを組み立てる
        var result = new StringBuilder();
        result.Append(ConvertUsings(templateFile));
        result.AppendLine($"namespace {nameSpaceName}");
        result.AppendLine("{");
        result.AppendLine($"  public partial class {className}");
        result.AppendLine("  {");
        result.AppendLine("    public string TransformText()");
        result.AppendLine("    {");
        result.AppendLine("      var template = new StringBuilder();");

        // メソッド内部の組み立て
        var indentIndex = 0;
        var type = Type.None;
        var templates = RemoveImport(templateFile).Split('\n');
        foreach (var line in templates)
        {
          var outputLine = line.Replace("\r", string.Empty);
          var outputLineTrimStart = outputLine.TrimStart();
          if (string.IsNullOrEmpty(outputLine))
            continue;

          switch (type)
          {
            case Type.None:
              // 1行のコード
              var oneLineCodeReg = "<# (.+?) #>";
              if (Regex.Matches(outputLine, oneLineCodeReg).Count > 0)
              {
                result.Append(new string(' ', indentIndex * 2));
                result.AppendLine($"      {GetRegex(outputLineTrimStart, oneLineCodeReg, "$1")}");
                continue;
              }

              // 複数行のコード
              if (Regex.Matches(outputLineTrimStart, "<#").Count > 0 && Regex.Matches(outputLineTrimStart, "<#=").Count == 0)
              {
                type = Type.CodeStart;
                continue;
              }
              break;
            case Type.CodeStart:
              if (Regex.Matches(outputLineTrimStart, "#>").Count > 0)
              {
                type = Type.None;
              }
              else
              {
                if (outputLineTrimStart.IndexOf("}") >= 0)
                {
                  indentIndex--;
                }

                result.Append(new string(' ', indentIndex * 2));
                result.AppendLine($"      {outputLineTrimStart}");

                if (outputLineTrimStart.IndexOf("{") >= 0)
                {
                  indentIndex++;
                }
              }
              continue;
          }

          var rgx = new Regex("({|})");
          outputLine = ConvertParams(ConvertPropertyBlock(outputLine));

          // 文字列補間式でエラーになる括弧のエスケープ
          if (rgx.Matches(outputLine).Count < 2)
          {
            outputLine = outputLine.Replace("{", "{{").Replace("}", "}}");
          }
          result.Append(new string(' ', indentIndex * 2));
          result.AppendLine($"      template.AppendLine($\"{outputLine}\");");
        }

        result.AppendLine("      return template.ToString();");
        result.AppendLine("    }");
        result.AppendLine("  }");
        result.AppendLine("}");

        // namespace、class、methodのソースコードの文字列を返す
        return result.ToString();
      }

      // usingを設定
      string ConvertUsings(string src)
      {
        var result = new StringBuilder();
        var matches = Regex.Matches(src, "<#@ import namespace=\"(.+?)\" #>");
        if (matches.Count > 0)
        {
          foreach (Match match in matches)
          {
            result.AppendLine(match.Result("using $1;"));
          }
          result.AppendLine();
        }

        return result.ToString();
      }

      // メソッド内部からInportを削除
      string RemoveImport(string src)
      {
        return GetRegex(src, "<#@ import namespace=\"(.+?)\" #>", string.Empty);
      }

      // パラメータを設定
      string ConvertParams(string src)
      {
        return GetRegex(src, "<#= (.+?) #>", "{$1}");
      }

      // プロパティブロックを設定
      string ConvertPropertyBlock(string src)
      {
        return GetRegex(src, " {(.+?)}", "{{$1}}");
      }

      // 指定した正規表現で文字列置き換え
      string GetRegex(string src, string reg, string replace)
      {
        return Regex.Replace(src, reg, replace);
      }
    }
  }
}
