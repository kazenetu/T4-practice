using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;

namespace LoadAssembly
{
  /// <summary>
  /// アセンブリ読み込み管理クラス
  /// </summary>
  public class AssemblyLoader: IAssemblyLoader,IDisposable
  {
    /// <summary>
    /// 読み込み済みアセンブリType
    /// </summary>
    protected List<Type> assemblyTypes = new List<Type>();

    /// <summary>
    /// アセンブリ
    /// </summary>
    private AssemblyLoadContext alc = null;

    /// <summary>
    /// アセンブリのロード
    /// </summary>
    /// <param name="filePath">ファイルの絶対パス</param>
    public void Load(string filePath)
    {
      // アセンブリの読み込み
      alc = new MyAssemblyLoadContext();
      var assembly = alc.LoadFromAssemblyPath(filePath);

      // リストに追加
      assemblyTypes.AddRange(assembly.GetTypes());
    }

    /// <summary>
    /// インターフェースインスタンス取得
    /// </summary>
    /// <typeparam name="T">インターフェース</typeparam>
    /// <returns>インターフェースインスタンスまたはnull</returns>
    public T GetInterfaceInstance<T>() where T : class
    {
      // 対象インターフェース名取得
      var typeName = typeof(T).FullName;

      // アセンブリからインターフェースを継承したクラスのTypeリストを取得
      var targets = assemblyTypes.Where(typeItem => typeItem.IsClass && typeItem.GetInterface(typeName) != null);
      if (!targets.Any())
      {
        return null;
      }

      // 最初のクラスTypeを取得
      var type = targets.First();
      if (type is null)
      {
        return null;

      }

      // インスタンスを返す
      return Activator.CreateInstance(type) as T;
    }

    /// <summary>
    /// 解放処理
    /// </summary>
    public void Dispose()
    {
      //alc.Unload();
    }
  }
}
