using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraPostEffect: MonoBehaviour
{

    public Material monoTone;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, monoTone);
    }
}