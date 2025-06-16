using UnityEngine;

/// <summary>
/// IPausable / IPausableAsync の自動登録を1つで対応する統合コンポーネント。
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(MonoBehaviour))]
public class AutoRegisterPausable : MonoBehaviour
{
    [Tooltip("このオブジェクトが属するポーズグループ。IPausableにのみ適用されます。")]
    public PauseGroup group;

    private IPausable pausable;
    private IPausableAsync asyncPausable;

    private void Awake()
    {
        pausable = GetComponent<IPausable>();
        if (pausable != null)
        {
            PauseManager.Register(pausable, group);
        }

        asyncPausable = GetComponent<IPausableAsync>();
        if (asyncPausable != null && (pausable == null || !(pausable is IPausableAsync)))
        {
            PauseManager.RegisterAsyncOnly(asyncPausable);
        }
    }

    private void OnDestroy()
    {
        if (pausable != null)
        {
            PauseManager.Unregister(pausable);
        }

        if (asyncPausable != null && (pausable == null || !(pausable is IPausableAsync)))
        {
            PauseManager.UnregisterAsyncOnly(asyncPausable);
        }
    }
}
