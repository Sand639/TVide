//using UnityEngine;

/// <summary>
/// 同期処理（Updateなど）をポーズ制御するためのインターフェース。
/// 例：プレイヤーや敵の入力・移動。
/// </summary>
public interface IPausable
{
    /// <summary>
    /// 現在ポーズ対象かどうか（falseならPauseManagerは無視）。
    /// </summary>
    bool IsPausable { get; }

    /// <summary>
    /// ポーズ状態になったときに呼ばれる処理。
    /// </summary>
    void OnPause();

    /// <summary>
    /// ポーズ解除時に呼ばれる処理。
    /// </summary>
    void OnResume();
}
