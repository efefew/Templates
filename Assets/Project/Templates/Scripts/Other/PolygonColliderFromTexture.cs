using System.Collections.Generic;
using System.Linq;
/*using AdvancedEditorTools.Attributes;*/
using UnityEditor;
using UnityEngine;

/// <summary>
/// Генерирует PolygonCollider2D по прозрачности спрайта (SpriteMode Single или Multiple)
/// Работает по альфа-каналу, можно вызывать в редакторе через контекстное меню.
/// </summary>
[RequireComponent(typeof(PolygonCollider2D))]
public class PolygonColliderFromTexture : MonoBehaviour
{
    [SerializeField, Tooltip("Спрайт, по которому строится коллайдер. Если не задан, берется из SpriteRenderer.")]
    private Sprite _sourceSprite;

    [SerializeField, Range(0f, 1f), Tooltip("Порог альфа-канала (пиксели >= threshold считаются непрозрачными)")]
    private float _alphaThreshold = 0.1f;

    [SerializeField, Range(1, 8), Tooltip("Фактор снижения разрешения. 1 = исходные пиксели, 2 = половина, 4 = в 4 раза меньше.")]
    private int _downSample = 1;

    [SerializeField, Min(0), Tooltip("Допуск для упрощения полигона (чем больше, тем меньше вершин)")]
    public float SimplifyTolerance = 0.44f;

    [SerializeField, Min(0), Tooltip("Масштаб коллайдера относительно исходного размера спрайта (1 = 100%)")]
    public float ColliderScale = 30f;

    /*[Button("Generate Collider", 25)]*/
    [ContextMenu("Generate Collider")]
    public void Generate()
    {
        AssetDatabase.StartAssetEditing();
        Sprite sprite = _sourceSprite;
        if (sprite == null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr) sprite = sr.sprite;
        }

        if (sprite == null)
        {
            Debug.LogError("Не найден спрайт для генерации коллайдера.");
            return;
        }

        Texture2D tex = sprite.texture;
        if (!tex.isReadable)
        {
            Debug.LogError("Текстура не доступна для чтения. Включи Read/Write Enabled в Import Settings.");
            return;
        }

        // 🔸 Учитываем SpriteMode Multiple — вырезаем область спрайта по sprite.rect
        Rect rect = sprite.rect;
        int xMin = Mathf.RoundToInt(rect.x);
        int yMin = Mathf.RoundToInt(rect.y);
        int width = Mathf.RoundToInt(rect.width);
        int height = Mathf.RoundToInt(rect.height);

        Color[] pixels = tex.GetPixels(xMin, yMin, width, height);

        // создаем временную текстуру только для нужного подспрайта
        Texture2D subTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        subTex.SetPixels(pixels);
        subTex.Apply();

        // ↓ Теперь работаем с subTex, а pivot — из sprite.pivot
        bool[,] grid = BuildAlphaGrid(subTex, _alphaThreshold, _downSample);
        var polygons = ExtractPolygons(grid, sprite, _downSample, SimplifyTolerance, ColliderScale);

        if (polygons.Count == 0)
        {
            Debug.LogWarning("Контур не найден в спрайте.");
            return;
        }

        PolygonCollider2D pc = GetComponent<PolygonCollider2D>();
        pc.pathCount = polygons.Count;
        for (int i = 0; i < polygons.Count; i++)
            pc.SetPath(i, polygons[i]);

        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        Debug.Log($"✅ Создан PolygonCollider2D: {polygons.Count} контур(ов), {polygons.Sum(p => p.Length)} точек (масштаб {ColliderScale:F2})");
    }

    // ======================================================================

    private static bool[,] BuildAlphaGrid(Texture2D tex, float alphaThreshold, int downsample)
    {
        int texW = tex.width;
        int texH = tex.height;
        int step = Mathf.Max(1, downsample);

        int gridW = texW / step;
        int gridH = texH / step;
        bool[,] grid = new bool[gridW, gridH];

        Color[] pixels = tex.GetPixels();

        for (int y = 0; y < gridH; y++)
        {
            for (int x = 0; x < gridW; x++)
            {
                int sx = Mathf.Clamp(x * step + step / 2, 0, texW - 1);
                int sy = Mathf.Clamp(y * step + step / 2, 0, texH - 1);
                Color c = pixels[sy * texW + sx];
                grid[x, y] = c.a >= alphaThreshold;
            }
        }

        return grid;
    }

    // ======================================================================

    private static List<Vector2[]> ExtractPolygons(bool[,] grid, Sprite sprite, int downsample, float simplifyTolerance, float scale)
    {
        int w = grid.GetLength(0);
        int h = grid.GetLength(1);

        var segments = MarchingSquares(grid, w, h);
        var loops = JoinSegmentsIntoLoops(segments);

        float ppu = sprite.pixelsPerUnit;
        Vector2 pivot = sprite.pivot; // в пикселях
        float pixelToUnit = 1f / ppu * downsample;

        return loops.Select(loop => loop.Select(p =>
                {
                    float px = p.x * downsample - pivot.x;
                    float py = p.y * downsample - pivot.y;
                    // применяем масштабирование
                    return new Vector2(px * pixelToUnit * scale, py * pixelToUnit * scale);
                })
                .ToArray())
            .Select(pts => RamerDouglasPeucker(pts, simplifyTolerance))
            .Where(simplified => simplified.Length >= 3)
            .ToList();
    }

    // ======================================================================
    // --- marching squares (контур по альфа-карте)

    private struct Segment { public Vector2Int A, B; public Segment(Vector2Int a, Vector2Int b) { A = a; B = b; } }

    private static List<Segment> MarchingSquares(bool[,] grid, int w, int h)
    {
        List<Segment> segments = new();
        for (int y = 0; y < h - 1; y++)
        {
            for (int x = 0; x < w - 1; x++)
            {
                int v = (grid[x, y] ? 1 : 0)
                        | (grid[x + 1, y] ? 2 : 0)
                        | (grid[x + 1, y + 1] ? 4 : 0)
                        | (grid[x, y + 1] ? 8 : 0);

                switch (v)
                {
                    case 1: segments.Add(new Segment(new Vector2Int(x, (int)(y + 0.5f)), new Vector2Int((int)(x + 0.5f), y))); break;
                    case 2: segments.Add(new Segment(new Vector2Int((int)(x + 0.5f), y), new Vector2Int((int)(x + 1f), (int)(y + 0.5f)))); break;
                    case 3: segments.Add(new Segment(new Vector2Int(x, (int)(y + 0.5f)), new Vector2Int((int)(x + 1f), (int)(y + 0.5f)))); break;
                    case 4: segments.Add(new Segment(new Vector2Int((int)(x + 1f), (int)(y + 0.5f)), new Vector2Int((int)(x + 0.5f), (int)(y + 1f)))); break;
                    case 5:
                        segments.Add(new Segment(new Vector2Int(x, (int)(y + 0.5f)), new Vector2Int((int)(x + 0.5f), y)));
                        segments.Add(new Segment(new Vector2Int((int)(x + 1f), (int)(y + 0.5f)), new Vector2Int((int)(x + 0.5f), (int)(y + 1f))));
                        break;
                    case 6: segments.Add(new Segment(new Vector2Int((int)(x + 0.5f), y), new Vector2Int((int)(x + 0.5f), (int)(y + 1f)))); break;
                    case 7: segments.Add(new Segment(new Vector2Int(x, (int)(y + 0.5f)), new Vector2Int((int)(x + 0.5f), (int)(y + 1f)))); break;
                    case 8: segments.Add(new Segment(new Vector2Int(x, (int)(y + 0.5f)), new Vector2Int((int)(x + 0.5f), (int)(y + 1f)))); break;
                    case 9: segments.Add(new Segment(new Vector2Int((int)(x + 0.5f), y), new Vector2Int((int)(x + 0.5f), (int)(y + 1f)))); break;
                    case 10:
                        segments.Add(new Segment(new Vector2Int(x, (int)(y + 0.5f)), new Vector2Int((int)(x + 0.5f), y)));
                        segments.Add(new Segment(new Vector2Int((int)(x + 1f), (int)(y + 0.5f)), new Vector2Int((int)(x + 0.5f), (int)(y + 1f))));
                        break;
                    case 11: segments.Add(new Segment(new Vector2Int((int)(x + 1f), (int)(y + 0.5f)), new Vector2Int((int)(x + 0.5f), (int)(y + 1f)))); break;
                    case 12: segments.Add(new Segment(new Vector2Int(x, (int)(y + 0.5f)), new Vector2Int((int)(x + 1f), (int)(y + 0.5f)))); break;
                    case 13: segments.Add(new Segment(new Vector2Int((int)(x + 0.5f), y), new Vector2Int((int)(x + 1f), (int)(y + 0.5f)))); break;
                    case 14: segments.Add(new Segment(new Vector2Int(x, (int)(y + 0.5f)), new Vector2Int((int)(x + 0.5f), y))); break;
                }
            }
        }
        return segments;
    }

    private static List<List<Vector2>> JoinSegmentsIntoLoops(List<Segment> segments)
    {
        var loops = new List<List<Vector2>>();
        var used = new HashSet<(Vector2Int, Vector2Int)>();
        var dict = new Dictionary<Vector2Int, List<Vector2Int>>();

        foreach (Segment s in segments)
        {
            if (!dict.ContainsKey(s.A)) dict[s.A] = new List<Vector2Int>();
            if (!dict.ContainsKey(s.B)) dict[s.B] = new List<Vector2Int>();
            dict[s.A].Add(s.B);
            dict[s.B].Add(s.A);
        }

        foreach (var kv in dict)
        {
            Vector2Int start = kv.Key;
            foreach (Vector2Int n in kv.Value)
            {
                if (used.Contains((start, n))) continue;
                var loop = new List<Vector2>();
                Vector2Int prev = start, cur = n;

                loop.Add(start);
                loop.Add(cur);
                used.Add((start, n));
                used.Add((n, start));

                int guard = 0;
                while (guard++ < 5000)
                {
                    var neigh = dict[cur];
                    Vector2Int next = neigh.FirstOrDefault(p => p != prev && !used.Contains((cur, p)));
                    if (next == default) break;

                    used.Add((cur, next));
                    used.Add((next, cur));

                    prev = cur;
                    cur = next;
                    loop.Add(cur);
                    if (cur == start) break;
                }

                if (loop.Count > 2)
                    loops.Add(loop.Select(v => v).ToList());
            }
        }

        return loops;
    }

    // ======================================================================
    // --- Упрощение полигона (Ramer–Douglas–Peucker)

    private static Vector2[] RamerDouglasPeucker(Vector2[] pts, float eps)
    {
        if (pts.Length < 3) return pts;
        List<Vector2> result = Rdp(pts, 0, pts.Length - 1, eps);
        return result.ToArray();
    }

    private static List<Vector2> Rdp(Vector2[] pts, int a, int b, float eps)
    {
        float maxDist = 0;
        int index = 0;
        for (int i = a + 1; i < b; i++)
        {
            float d = PerpDistance(pts[i], pts[a], pts[b]);
            if (d > maxDist)
            {
                maxDist = d;
                index = i;
            }
        }
        if (maxDist > eps)
        {
            var left = Rdp(pts, a, index, eps);
            var right = Rdp(pts, index, b, eps);
            left.RemoveAt(left.Count - 1);
            left.AddRange(right);
            return left;
        }
        else return new List<Vector2> { pts[a], pts[b] };
    }

    private static float PerpDistance(Vector2 p, Vector2 a, Vector2 b)
    {
        if (a == b) return Vector2.Distance(p, a);
        float t = Vector2.Dot(p - a, b - a) / (b - a).sqrMagnitude;
        Vector2 proj = a + t * (b - a);
        return Vector2.Distance(p, proj);
    }
}
