using Interface;
using LoadAssembly;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
    {
        private List<IOutputs> outpus = new List<IOutputs>();

        public MainWindow()
        {
            InitializeComponent();

            // 読み込み対象のdllのフルパス設定
            var thisPath = Path.GetDirectoryName(System.AppContext.BaseDirectory);
            var assemblyPath = Path.Combine(thisPath, "lib");

            // DLL読み込み
            var dllFiles = Directory.GetFiles(assemblyPath, "*.dll");
            var loadResult = new StringBuilder(dllFiles.Length);
            foreach (var filePath in dllFiles)
            {
                // Interface.dllは読み込み対象外
                if (filePath.EndsWith("Interface.dll"))
                    continue;

                using (var loader = new AssemblyLoader())
                {
                    loader.Load(filePath);
                    outpus.Add(loader.GetInterfaceInstance<IOutputs>());
                    loadResult.AppendLine($"{filePath}をロードしました。");
                }
            }

            Result.Text = loadResult.ToString();
        }


        private void Run_Click(object sender, RoutedEventArgs e)
        {
            var result = new StringBuilder();
            foreach (var output in outpus)
            {
                // 実行結果を格納
                result.AppendLine(output.Output());
            }

            // 実行結果を反映
            Result.Text = result.ToString();
        }
    }
}
