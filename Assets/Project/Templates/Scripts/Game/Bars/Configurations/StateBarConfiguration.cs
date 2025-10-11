using UnityEngine;

[CreateAssetMenu(fileName = "StateBarConfiguration", menuName = "Bars/StateBarConfiguration")]
public class StateBarConfiguration : ScriptableObject
{
    public float MaxValue;
    public bool Destroyable;
    public SliderBarUI Prefab;
}
