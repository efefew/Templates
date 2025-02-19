using System.Collections.Generic;

public class KeyCombination
{
    //private bool _needOrder;
    private Key[] _keys;
    public KeyCombination(params Key[] keys) => _keys = keys;
    public KeyCombination(List<Key> keys) => _keys = keys.ToArray();
    public float Get()
    {
        float? min = null;
        for (int idKey = 0; idKey < _keys.Length; idKey++)
        {
            float value = _keys[idKey].Get();
            if (min > value || min == null)
            {
                min = value;
                if (min == 0)
                {
                    break;
                }
            }
        }

        min ??= 0;

        return (float)min;
    }
}
