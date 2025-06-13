using UnityEngine;
using System.Collections;
public class HitStopManager : MonoBehaviour
{
    // ‚Ç‚±‚©‚ç‚Å‚àŒÄ‚Ño‚¹‚é‚æ‚¤‚É‚·‚é
    public static HitStopManager instance;
    private void Start()
    {
        instance = this;
    }

    public void StartHitStop(float duration)
    {
        StartCoroutine(HitStopCoroutine(duration));

        // ƒJƒƒ‰U“®‚ğŒÄ‚Ño‚·
        if (HitStopCameraShake.instance != null)
        {
            HitStopCameraShake.instance.ShakeDuringHitStop(duration, 0.3f);
        }
    }


    private IEnumerator HitStopCoroutine(float duration)
    { 
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }


}