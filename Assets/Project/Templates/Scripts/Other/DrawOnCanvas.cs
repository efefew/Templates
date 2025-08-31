using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class DrawOnCanvas : MonoBehaviour
{
    [Header("Canvas (texture) settings")]
    public int TextureWidth = 1024;
    public int TextureHeight = 1024;
    public Color Background = Color.white;

    [Header("Brush")] public Color BrushColor = Color.black;
    [Range(1, 128)] public int BrushSize = 16;
    private const bool ERASER = false;

    private RawImage _raw;
    private Texture2D _tex;
    private RectTransform _rt;

    private bool _drawing;
    private Vector2 _prevUV;
    private Color32[] _bgCache;

    private void Awake()
    {
        _raw = GetComponent<RawImage>();
        _rt = (RectTransform)transform;
        _tex = new Texture2D(TextureWidth, TextureHeight, TextureFormat.RGBA32, false)
        {
            wrapMode = TextureWrapMode.Clamp
        };
        _bgCache = new Color32[TextureWidth * TextureHeight];
        for (int i = 0; i < _bgCache.Length; i++) _bgCache[i] = Background;
        _tex.SetPixels32(_bgCache);
        _tex.Apply();
        _raw.texture = _tex;
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetMouseButtonDown(0)) StartDraw(ScreenPointToUV(Input.mousePosition));
        if (Input.GetMouseButton(0))      ContinueDraw(ScreenPointToUV(Input.mousePosition));
        if (Input.GetMouseButtonUp(0))    EndDraw();
#else
        if (Input.touchCount > 0)
        {
            var t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began) StartDraw(ScreenPointToUV(t.position));
            else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary) ContinueDraw(ScreenPointToUV(t.position));
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) EndDraw();
        }
#endif
    }

    private Vector2 ScreenPointToUV(Vector2 screenPos)
    {
        // В UV (0..1) внутри RawImage
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rt, screenPos, null, out Vector2 local))
            return new Vector2(-1, -1);
        Rect r = _rt.rect;
        float u = Mathf.InverseLerp(r.xMin, r.xMax, local.x);
        float v = Mathf.InverseLerp(r.yMin, r.yMax, local.y);
        return new Vector2(u, v);
    }

    private void StartDraw(Vector2 uv)
    {
        if (!Inside(uv)) return;
        _drawing = true;
        _prevUV = uv;
        DrawDotUV(uv);
        _tex.Apply();
    }

    private void ContinueDraw(Vector2 uv)
    {
        if (!_drawing || !Inside(uv)) return;
        DrawLineUV(_prevUV, uv);
        _prevUV = uv;
        _tex.Apply();
    }

    private void EndDraw() => _drawing = false;

    private static bool Inside(Vector2 uv) => uv.x is >= 0 and <= 1 && uv.y is >= 0 and <= 1;

    // ====== Рисование в пикселях ======
    private void DrawDotUV(Vector2 uv) => DrawCircle(UVtoPx(uv), BrushSize, ERASER ? Background : BrushColor);

    private void DrawLineUV(Vector2 a, Vector2 b)
    {
        Vector2Int p0 = UVtoPx(a);
        Vector2Int p1 = UVtoPx(b);
        int steps = Mathf.CeilToInt(Vector2.Distance(p0, p1));
        for (int i = 0; i <= steps; i++)
        {
            float t = steps == 0 ? 0 : (float)i / steps;
            Vector2Int p = Vector2Int.RoundToInt(Vector2.Lerp(p0, p1, t));
            DrawCircle(p, BrushSize, ERASER ? Background : BrushColor);
        }
    }

    private Vector2Int UVtoPx(Vector2 uv) => new Vector2Int(
        Mathf.Clamp(Mathf.RoundToInt(uv.x * (_tex.width  - 1)), 0, _tex.width  - 1),
        Mathf.Clamp(Mathf.RoundToInt((uv.y) * (_tex.height - 1)), 0, _tex.height - 1)
    );

    private void DrawCircle(Vector2Int center, int radius, Color color)
    {
        int r2 = radius * radius;
        int x0 = Mathf.Max(center.x - radius, 0);
        int x1 = Mathf.Min(center.x + radius, _tex.width - 1);
        int y0 = Mathf.Max(center.y - radius, 0);
        int y1 = Mathf.Min(center.y + radius, _tex.height - 1);

        for (int y = y0; y <= y1; y++)
        {
            int dy = y - center.y;
            for (int x = x0; x <= x1; x++)
            {
                int dx = x - center.x;
                if (dx * dx + dy * dy <= r2)
                    _tex.SetPixel(x, y, color);
            }
        }
    }

    // Вспомогательно: очистка холста
    [ContextMenu("Clear")]
    public void Clear()
    {
        _tex.SetPixels32(_bgCache);
        _tex.Apply();
    }
}
