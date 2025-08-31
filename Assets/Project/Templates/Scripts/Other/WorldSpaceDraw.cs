using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(RawImage))]
public class WorldSpaceDraw : MonoBehaviour
{
    private Camera _raycastCamera;
    public Color BrushColor = Color.black; 
    [Range(1, 128)] public int BrushSize = 16;
    public Color Background = Color.white;
    public int TextureWidth = 1024;
    public int TextureHeight = 1024;

    private Texture2D _tex;
    private RawImage _raw;
    private bool _drawing;
    private Vector2 _prevUV;
    private Color32[] _bgCache;

    private void Awake()
    {
        _raycastCamera  = Camera.main;
        _raw = GetComponent<RawImage>();

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
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0)) StartDraw(Input.mousePosition);
        if (Input.GetMouseButton(0)) ContinueDraw(Input.mousePosition);
        if (Input.GetMouseButtonUp(0)) EndDraw();
#else
        if (Input.touchCount > 0)
        {
            var t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began) StartDraw(t.position);
            else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary) ContinueDraw(t.position);
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) EndDraw();
        }
#endif
    }

    private void StartDraw(Vector2 screenPos)
    {
        if (TryGetUV(screenPos, out Vector2 uv))
        {
            _drawing = true;
            _prevUV = uv;
            DrawDotUV(uv);
            _tex.Apply();
        }
    }

    private void ContinueDraw(Vector2 screenPos)
    {
        if (!_drawing) return;
        if (TryGetUV(screenPos, out Vector2 uv))
        {
            DrawLineUV(_prevUV, uv);
            _prevUV = uv;
            _tex.Apply();
        }
    }

    private void EndDraw() => _drawing = false;

    // Получение UV через Raycast
    private bool TryGetUV(Vector2 screenPos, out Vector2 uv)
    {
        PointerEventData ped = new(EventSystem.current)
        {
            position = screenPos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);

        foreach (RaycastResult r in results)
        {
            if (r.gameObject == gameObject) // попали в этот RawImage
            {
                RectTransform rt = (RectTransform)transform;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPos, _raycastCamera, out Vector2 localPoint))
                {
                    Rect rect = rt.rect;
                    float u = Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x);
                    float v = Mathf.InverseLerp(rect.yMin, rect.yMax, localPoint.y);
                    uv = new Vector2(u, v);
                    return true;
                }
            }
        }

        uv = Vector2.zero;
        return false;
    }

    // Рисование
    private void DrawDotUV(Vector2 uv) => DrawCircle(UVtoPx(uv), BrushSize, BrushColor);

    private void DrawLineUV(Vector2 a, Vector2 b)
    {
        Vector2Int p0 = UVtoPx(a);
        Vector2Int p1 = UVtoPx(b);
        int steps = Mathf.CeilToInt(Vector2.Distance(p0, p1));
        for (int i = 0; i <= steps; i++)
        {
            float t = steps == 0 ? 0 : (float)i / steps;
            Vector2Int p = Vector2Int.RoundToInt(Vector2.Lerp(p0, p1, t));
            DrawCircle(p, BrushSize, BrushColor);
        }
    }

    private Vector2Int UVtoPx(Vector2 uv) => new(
        Mathf.Clamp(Mathf.RoundToInt(uv.x * (_tex.width - 1)), 0, _tex.width - 1),
        Mathf.Clamp(Mathf.RoundToInt(uv.y * (_tex.height - 1)), 0, _tex.height - 1)
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

    [ContextMenu("Clear")]
    public void Clear()
    {
        _tex.SetPixels32(_bgCache);
        _tex.Apply();
    }
}
