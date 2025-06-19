using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ポーズ状態の制御と、登録されたオブジェクトへの通知を行うマネージャ。
/// </summary>
public class PauseManager : MonoBehaviour
{
    private static readonly Dictionary<IPausable, PauseGroup> pausableMap = new();
    private static readonly List<IPausableAsync> asyncPausables = new();

    public static bool IsPaused { get; private set; } = false;

    // 同期処理とグループ情報を含めて登録
    public static void Register(IPausable pausable, PauseGroup group)
    {
        if (!pausableMap.ContainsKey(pausable))
        {
            pausableMap.Add(pausable, group);

            // 非同期インターフェースを兼ねている場合
            if (pausable is IPausableAsync async && !asyncPausables.Contains(async))
            {
                asyncPausables.Add(async);
            }
        }
    }

    // 非同期のみを直接登録（IPausableを実装していない場合）
    public static void RegisterAsyncOnly(IPausableAsync async)
    {
        if (!asyncPausables.Contains(async))
            asyncPausables.Add(async);
    }

    public static void Unregister(IPausable pausable)
    {
        pausableMap.Remove(pausable);

        if (pausable is IPausableAsync async)
        {
            asyncPausables.Remove(async);
        }
    }

    public static void UnregisterAsyncOnly(IPausableAsync async)
    {
        asyncPausables.Remove(async);
    }

    // 全体ポーズ
    public static void PauseAll()
    {
        if (IsPaused) return;
        IsPaused = true;
        Time.timeScale = 0;

        foreach (var pair in pausableMap)
        {
            if (pair.Key.IsPausable)
                pair.Key.OnPause();
        }

        foreach (var async in asyncPausables)
        {
            async.OnPauseAsync();
        }
    }

    // 全体再開
    public static void ResumeAll()
    {
        if (!IsPaused) return;
        IsPaused = false;
        Time.timeScale = 1;

        foreach (var pair in pausableMap)
        {
            if (pair.Key.IsPausable)
                pair.Key.OnResume();
        }

        foreach (var async in asyncPausables)
        {
            async.OnResumeAsync();
        }
    }

    // グループ単位のポーズ（親階層含む）
    public static void PauseGroup(PauseGroup group)
    {
        foreach (var pair in pausableMap)
        {
            if (IsInGroupHierarchy(pair.Value, group) && pair.Key.IsPausable)
                pair.Key.OnPause();
        }
    }

    // グループ単位の再開（親階層含む）
    public static void ResumeGroup(PauseGroup group)
    {
        foreach (var pair in pausableMap)
        {
            if (IsInGroupHierarchy(pair.Value, group) && pair.Key.IsPausable)
                pair.Key.OnResume();
        }
    }

    // グループ階層関係チェック
    private static bool IsInGroupHierarchy(PauseGroup current, PauseGroup target)
    {
        while (current != null)
        {
            if (current == target)
                return true;
            current = current.parentGroup;
        }
        return false;
    }
}
