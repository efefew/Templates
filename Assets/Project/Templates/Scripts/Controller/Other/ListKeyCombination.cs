using System.Collections.Generic;

public class ListKeyCombination
{
    private KeyCombination[] _keyCombinations;
    public ListKeyCombination(params Key[] keys) => _keyCombinations = new KeyCombination[] { new(keys) };
    public ListKeyCombination(params KeyCombination[] keyCombinations) => _keyCombinations = keyCombinations;
    public ListKeyCombination(List<KeyCombination> keyCombinations) => _keyCombinations = keyCombinations.ToArray();
    public float Get()
    {
        float? max = null;
        for (int idCombination = 0; idCombination < _keyCombinations.Length; idCombination++)
        {
            float value = _keyCombinations[idCombination].Get();
            if (max < value || max == null)
            {
                max = value;
                if (max == 1)
                {
                    break;
                }
            }
        }

        max ??= 0;
        return (float)max;
    }
}
