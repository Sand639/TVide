using UnityEngine;

/// <summary>
/// 一時停止対象を分類するためのScriptableObject（UI、プレイヤー、敵など）。
/// 階層構造も可能で、親が停止されると子グループも止まる。
/// </summary>
[CreateAssetMenu(menuName = "Pause/Pause Group")]
public class PauseGroup : ScriptableObject
{
    [Tooltip("グループの識別名（表示・デバッグ用）")]
    public string groupName;

    [Tooltip("親グループ。設定すると階層的な制御が可能。")]
    public PauseGroup parentGroup;
}
