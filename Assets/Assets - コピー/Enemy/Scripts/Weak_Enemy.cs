using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

#if UNIYT_EDITOR

[CostomEditor(typeof(Enemy_Base))]
#endif

public class Weak_Enemy : Enemy_Base
{
    public override void Move()
    {
        if (GetMove())
        {
            // ˆÚ“®
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
        }
    }
}
