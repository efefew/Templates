using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static Health;

[CreateAssetMenu(fileName = "HealthBarConfiguration", menuName = "Bars/HealthBarConfiguration")]
public class HealthBarConfiguration : StateBarConfiguration
{
    public List<Resist> Resists;
}
