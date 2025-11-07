#region

using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

#endregion

[AddComponentMenu("Condition/Timer")]
public class Timer : Condition
{

    [FormerlySerializedAs("time")] [Min(0)] [SerializeField]
    private float _time;

    private IEnumerator IGetCondition()
    {
        yield return new WaitForSeconds(_time);
    }

    public override IEnumerator GetCondition()
    {
        return IGetCondition();
    }
}