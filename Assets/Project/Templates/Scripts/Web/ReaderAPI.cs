using UnityEngine;

public abstract class ReaderAPI : MonoBehaviour
{
    protected abstract void AppendData();

    public abstract void StartData();
    public abstract void ClearData();
}
