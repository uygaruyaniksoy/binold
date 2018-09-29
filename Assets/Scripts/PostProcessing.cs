using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessing : MonoBehaviour {
    public Material RenderMaterial;
    
    public Camera GlowCamera;
    
    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, RenderMaterial);
    }
}
