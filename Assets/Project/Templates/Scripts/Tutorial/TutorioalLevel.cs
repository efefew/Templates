using System.Collections;

using UnityEngine;

public abstract class TutorioalLevel : MonoBehaviour
{
    protected Tutorial _tutorial;
    public virtual void StartTutorioal() => _tutorial = Tutorial.Instance;
    protected abstract IEnumerator ILevel();
}
