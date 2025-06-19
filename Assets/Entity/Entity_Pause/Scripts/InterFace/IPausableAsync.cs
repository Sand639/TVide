/// <summary>
/// 非同期処理（コルーチンやTween）をポーズ制御するためのインターフェース。
/// </summary>
public interface IPausableAsync
{
    /// <summary>
    /// 非同期処理の一時停止。
    /// </summary>
    void OnPauseAsync();

    /// <summary>
    /// 非同期処理の再開。
    /// </summary>
    void OnResumeAsync();
}
