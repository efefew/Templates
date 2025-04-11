using System.Globalization;
using UnityEngine;

public class GraphCreator : MonoBehaviour
{
    private LineRenderer _axisX, _axisY;

    private Vector3 _screenPos;
    private Vector3 _pos;

    private bool _created;
    [SerializeField]
    private GameObject _arrowPrefab;
    [SerializeField]
    private LineRenderer _axisPrefab;

    [SerializeField]
    [Min(0)]
    private int _countValueX, _countValueY;

    [SerializeField]
    private float _globalAxisX, _globalAxisY;

    [SerializeField]
    private Vector2 _globalAxisZero;
    [SerializeField]
    private float _minAxisX, _minAxisY, _maxAxisX, _maxAxisY;
    
    private ValueConteiner _targetValueX, _targetValueY;
    [SerializeField]
    private Transform _tr;
    [SerializeField]
    private ValueConteiner _valuePrefabX, _valuePrefabY;

    private Camera _c;


    #region Methods

    private void Start()
    {
        _c = Camera.main;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value">значение от 0 до 1</param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private float ConvertToOtherSystem(float value, float min, float max) => (value * (max - min)) + min;

    private void OnGUI()
    {
        Event e = Event.current;
        Vector2 mousePos = new()
        {
            x = e.mousePosition.x,
            y = _c.pixelHeight - e.mousePosition.y
        };
        _screenPos = new Vector3(mousePos.x, mousePos.y, _c.nearClipPlane);
        _pos = _c.ScreenToWorldPoint(_screenPos);
    }

    private void OnDisable() => Clear();

    private void Clear()
    {
        _tr.Clear();
        _created = false;
    }

    private void Update()
    {
        if (!_created) return;
        if (_pos.x < _globalAxisZero.x)
        {
            _targetValueX.transform.position = new Vector2(_globalAxisZero.x, _globalAxisZero.y);
            _targetValueX.text.text = Round(_minAxisX, 2);
        }
        else if (_pos.x > _globalAxisX)
        {
            _targetValueX.transform.position = new Vector2(_globalAxisX, _globalAxisZero.y);
            _targetValueX.text.text = Round(_maxAxisX, 2);
        }
        else
        {
            _targetValueX.transform.position = new Vector2(_pos.x, _globalAxisZero.y);
            _targetValueX.text.text = Round(ConvertToOtherSystem((_pos.x / _globalAxisX) - _globalAxisZero.x, _minAxisX, _maxAxisX), 2);
        }

        if (_pos.y < _globalAxisZero.y)
        {
            _targetValueY.transform.position = new Vector2(_globalAxisZero.x, _globalAxisZero.y);
            _targetValueY.text.text = Round(_minAxisY, 2);
        }
        else if (_pos.y > _globalAxisY)
        {
            _targetValueY.transform.position = new Vector2(_globalAxisZero.x, _globalAxisY);
            _targetValueY.text.text = Round(_maxAxisY, 2);
        }
        else
        {
            _targetValueY.transform.position = new Vector2(_globalAxisZero.x, _pos.y);
            _targetValueY.text.text = Round(ConvertToOtherSystem((_pos.y / _globalAxisY) - _globalAxisZero.y, _minAxisY, _maxAxisY), 2);
        }
    }

    private void CreateAxis()
    {
        float width = _axisPrefab.startWidth;
        float add = 1;
        _axisX = Instantiate(_axisPrefab, _tr);
        _axisX.SetColor(Color.black);
        _axisX.positionCount = 2;
        _axisX.SetPosition(0, new Vector2(_globalAxisZero.x - (width / 2), _globalAxisZero.y));
        _axisX.SetPosition(1, new Vector2(_globalAxisX + add, _globalAxisZero.y));

        _axisY = Instantiate(_axisPrefab, _tr);
        _axisY.SetColor(Color.black);
        _axisY.positionCount = 2;
        _axisY.SetPosition(0, new Vector2(_globalAxisZero.x, _globalAxisZero.y - (width / 2)));
        _axisY.SetPosition(1, new Vector2(_globalAxisZero.x, _globalAxisY + add));

        _ = Instantiate(_arrowPrefab, new Vector2(_globalAxisZero.x, _globalAxisY + add), Quaternion.Euler(0, 0, 0), _tr);
        _ = Instantiate(_arrowPrefab, new Vector2(_globalAxisX + add, _globalAxisZero.y), Quaternion.Euler(0, 0, -90), _tr);
    }

    private void CreateDiagram(LineRenderer line, Vector2[] points)
    {
        line.positionCount = points.Length;
        float scaleX = (_globalAxisX - _globalAxisZero.x) / (_maxAxisX - _minAxisX);
        float scaleY = (_globalAxisY - _globalAxisZero.y) / (_maxAxisY - _minAxisY);
        for (int id = 0; id < points.Length; id++)
            line.SetPosition(id, new Vector3((points[id].x - _minAxisX) * scaleX, (points[id].y - _minAxisY) * scaleY));
    }

    private void CreateValues()
    {
        float value;
        for (int id = 0; id < _countValueX; id++)
        {
            value = id / (float)(_countValueX - 1);
            _ = CreateValueX(ConvertToOtherSystem(value, _minAxisX, _maxAxisX), ConvertToOtherSystem(value, _globalAxisZero.x, _globalAxisX));
        }

        for (int id = 0; id < _countValueY; id++)
        {
            value = id / (float)(_countValueY - 1);
            _ = CreateValueY(ConvertToOtherSystem(value, _minAxisY, _maxAxisY), ConvertToOtherSystem(value, _globalAxisZero.y, _globalAxisY));
        }
    }

    private ValueConteiner CreateValueX(float value, float globalValue)
    {
        ValueConteiner valueLabel = Instantiate(_valuePrefabX, new Vector2(globalValue, _globalAxisZero.y), Quaternion.identity, _tr);
        valueLabel.text.text = Round(value, 2);
        return valueLabel;
    }

    private ValueConteiner CreateValueY(float value, float globalValue)
    {
        ValueConteiner valueLabel = Instantiate(_valuePrefabY, new Vector2(_globalAxisZero.x, globalValue), Quaternion.identity, _tr);
        valueLabel.text.text = Round(value, 2);
        return valueLabel;
    }

    private static string Round(float f, int countDecimalPlace) => System.Math.Round(f, countDecimalPlace).ToString(CultureInfo.InvariantCulture);

    private void Create()
    {
        CreateAxis();
        CreateValues();
        _created = true;
    }

    public void Create(float minX, float minY, float maxX, float maxY, LineRenderer[] lines, Vector2[][] points)
    {
        _minAxisX = minX;
        _minAxisY = minY;
        _maxAxisX = maxX;
        _maxAxisY = maxY;

        Create();
        for (int idLine = 0; idLine < lines.Length; idLine++)
            CreateDiagram(lines[idLine], points[idLine]);
    }

    public void Create(LineRenderer[] lines, Vector2[][] points)
    {
        float minX, minY, maxX, maxY;
        (minX, minY, maxX, maxY) = points[0].MinMax();
        _minAxisX = minX;
        _minAxisY = minY;
        _maxAxisX = maxX;
        _maxAxisY = maxY;
        if (points.Length > 0)
        {
            for (int id = 0; id < points.Length; id++)
            {
                (minX, minY, maxX, maxY) = points[id].MinMax();
                if (minX < _minAxisX)
                    _minAxisX = minX;
                if (maxX > _maxAxisX)
                    _maxAxisX = maxX;
                if (minY < _minAxisY)
                    _minAxisY = minY;
                if (maxY > _maxAxisY)
                    _maxAxisY = maxY;
            }
        }

        Create();
        for (int idLine = 0; idLine < lines.Length; idLine++)
            CreateDiagram(lines[idLine], points[idLine]);
    }

    public void Create(float minX, float minY, float maxX, float maxY, LineRenderer line, Vector2[] points)
    {
        _minAxisX = minX;
        _minAxisY = minY;
        _maxAxisX = maxX;
        _maxAxisY = maxY;

        (_minAxisX, _minAxisY, _maxAxisX, _maxAxisY) = points.MinMax();
        Create();
        CreateDiagram(line, points);
    }

    public void Create(LineRenderer line, Vector2[] points)
    {
        (_minAxisX, _minAxisY, _maxAxisX, _maxAxisY) = points.MinMax();
        Create();
        CreateDiagram(line, points);
    }

    #endregion Methods
}