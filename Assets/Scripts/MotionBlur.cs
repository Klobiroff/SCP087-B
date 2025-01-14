using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MotionBlur : MonoBehaviour
{
    [Range(0.0f, 0.9f)]
    public float blurAmount = 0.8f;
    
    [Range(0.0f, 0.1f)]
    public float separation = 0.02f;

    [Range(1, 4)]
    public float downsample = 1.5f;
    
    private Material material;
    private RenderTexture accumulationTexture;

    private void OnEnable()
    {
        material = new Material(Shader.Find("Hidden/MotionBlur"))
        {
            hideFlags = HideFlags.HideAndDontSave
        };
    }

    private void OnDisable()
    {
        DestroyImmediate(material);
        if (accumulationTexture != null)
        {
            DestroyImmediate(accumulationTexture);
            accumulationTexture = null;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        int width = (int)(source.width / downsample);
        int height = (int)(source.height / downsample);

        // Create accumulation texture if it doesn't exist or if size changed
        if (accumulationTexture == null || accumulationTexture.width != width || accumulationTexture.height != height)
        {
            DestroyImmediate(accumulationTexture);
            accumulationTexture = new RenderTexture(width, height, 0, source.format);
            accumulationTexture.filterMode = FilterMode.Bilinear;
            accumulationTexture.hideFlags = HideFlags.HideAndDontSave;
            Graphics.Blit(source, accumulationTexture);
        }

        // Set the properties
        material.SetFloat("_BlurAmount", blurAmount);
        material.SetFloat("_Separation", separation);

        // Create temporary downsampled texture
        RenderTexture temp = RenderTexture.GetTemporary(width, height, 0, source.format);
        temp.filterMode = FilterMode.Bilinear;

        // Downsample
        Graphics.Blit(source, temp);

        // Apply effect
        material.SetTexture("_AccumulationTex", accumulationTexture);
        Graphics.Blit(temp, destination, material);

        // Copy the result to accumulation
        Graphics.Blit(destination, accumulationTexture);

        // Release temporary texture
        RenderTexture.ReleaseTemporary(temp);
    }
}
