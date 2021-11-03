namespace t4_practice
{
    /// <summary>
    /// Repositoryのスーパークラス
    /// </summary>
    public interface ITransformText
    {
        /// <summary>
        /// T4ファイルから文字列に変換変換
        /// </summary>
        /// <returns>文字列</returns>
        string TransformText() { return "no Generated"; }
    }
}