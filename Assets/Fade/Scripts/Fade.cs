/*
 The MIT License (MIT)

Copyright (c) 2013 yamamura tatsuhiko

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class Fade : MonoBehaviour
{
	IFade fade;

	void Start ()
	{
		Init ();
		fade.Range = cutoutRange;
	}

	float cutoutRange;

    void Init()
    {
        fade = GetComponent<IFade>();
        if (fade == null)
        {
            Debug.LogError("IFade インターフェースを持つコンポーネントが見つかりません！");
            return;
        }

        fade.Range = cutoutRange; // 初期値を適用
        Debug.Log("Fade Initialized - Cutout Range: " + cutoutRange);
    }

    IEnumerator FadeoutCoroutine(float time, System.Action action)
    {
        float startTime = Time.time;
        float endTime = startTime + time;
        var endFrame = new WaitForEndOfFrame();

        while (Time.time <= endTime)
        {
            cutoutRange = Mathf.Clamp01(1 - (Time.time - startTime) / time); // 1 → 0 に変化
            fade.Range = cutoutRange;
            Debug.Log("Fade Out - Cutout Range: " + cutoutRange);
            yield return endFrame;
        }

        cutoutRange = 0;
        fade.Range = cutoutRange;

        action?.Invoke();
    }

    IEnumerator FadeinCoroutine(float time, System.Action action)
    {
        float startTime = Time.time;
        float endTime = startTime + time;
        var endFrame = new WaitForEndOfFrame();

        while (Time.time <= endTime)
        {
            cutoutRange = Mathf.Clamp01((Time.time - startTime) / time); // 0 → 1 に変化
            fade.Range = cutoutRange;
            Debug.Log("Fade In - Cutout Range: " + cutoutRange);
            yield return endFrame;
        }

        cutoutRange = 1;
        fade.Range = cutoutRange;

        action?.Invoke();
    }

    public Coroutine FadeOut (float time, System.Action action)
	{
		StopAllCoroutines ();
		return StartCoroutine (FadeoutCoroutine (time, action));
	}

	public Coroutine FadeOut (float time)
	{
		return FadeOut (time, null);
	}

	public Coroutine FadeIn (float time, System.Action action)
	{
		StopAllCoroutines ();
		return StartCoroutine (FadeinCoroutine (time, action));
	}

	public Coroutine FadeIn (float time)
	{
		return FadeIn (time, null);
	}
}