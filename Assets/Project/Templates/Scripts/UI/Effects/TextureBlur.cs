using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SpriteRenderer))]
public class TextureBlur : MonoBehaviour
{
    [Range(1, 100)] public int Iterations = 3;
    public Shader BlurShader;
    private Material _blurMaterial;

    private void OnEnable()
    {
        ApplyBlur();
    }

    private void ApplyBlur()
    {
        if (BlurShader == null || !BlurShader.isSupported)
            return;

        _blurMaterial = new Material(BlurShader);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Texture2D originalTexture = spriteRenderer.sprite.texture;

        // Создаем временную RenderTexture
        RenderTexture rt = RenderTexture.GetTemporary(originalTexture.width, originalTexture.height);
        Graphics.Blit(originalTexture, rt);

        // Применяем размытие
        for (int i = 0; i < Iterations; i++)
        {
            RenderTexture rt2 = RenderTexture.GetTemporary(rt.width, rt.height);
            Graphics.Blit(rt, rt2, _blurMaterial);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }

        // Конвертируем обратно в Texture2D
        Texture2D blurredTexture = new Texture2D(originalTexture.width, originalTexture.height);
        blurredTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        blurredTexture.Apply();

        // Обновляем спрайт
        spriteRenderer.sprite = Sprite.Create(blurredTexture, 
            new Rect(0, 0, blurredTexture.width, blurredTexture.height), 
            new Vector2(0.5f, 0.5f));

        RenderTexture.ReleaseTemporary(rt);
    }
}