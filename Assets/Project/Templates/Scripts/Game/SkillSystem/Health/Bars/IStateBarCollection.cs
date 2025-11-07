using System.Collections.Generic;
using UnityEngine;

public interface IStateBarCollection<T> where T : StateBar
{
    public List<T> Bars { get;}
}
