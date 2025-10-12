using BreakInfinity;
using UnityEngine;

[CreateAssetMenu(fileName = "StateBarConfiguration", menuName = "Bars/StateBarConfiguration")]
public class StateBarConfiguration : ScriptableObject
{
    public BigDouble MaxValue;
    public bool Destroyable;
    public SliderBarUI Prefab;
}