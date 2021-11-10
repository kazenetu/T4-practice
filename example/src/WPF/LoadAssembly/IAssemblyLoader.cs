namespace LoadAssembly
{
  /// <summary>
  /// アセンブリ読み込み管理インターフェイス
  /// </summary>
  public interface IAssemblyLoader
  {
    /// <summary>
    /// アセンブリのロード
    /// </summary>
    /// <param name="filePath">ファイルの絶対パス</param>
    void Load(string filePath);

    /// <summary>
    /// インターフェースインスタンス取得
    /// </summary>
    /// <typeparam name="T">インターフェース</typeparam>
    /// <returns>インターフェースインスタンスまたはnull</returns>
    public T GetInterfaceInstance<T>() where T : class;
  }
}
