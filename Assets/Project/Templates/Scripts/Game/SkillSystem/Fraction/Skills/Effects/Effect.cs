using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public abstract void Run(Unit initiator, Unit target);
}
