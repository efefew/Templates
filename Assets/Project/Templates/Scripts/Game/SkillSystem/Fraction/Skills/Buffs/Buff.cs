using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    public abstract void Run(Unit initiator, Unit target);
}
