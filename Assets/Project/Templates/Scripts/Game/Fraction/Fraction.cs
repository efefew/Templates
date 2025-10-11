using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "Fraction", menuName = "Global parameters/Fraction")]
public class Fraction : ScriptableObject
{
    
    [field:SerializeField, SerializedDictionary("Фракция", "Отношение")]
    public SerializedDictionary<Fraction, Relationship> Fractions {get; private set;}
    [field:SerializeField] public Relationship DefaultRelationship {get; private set;}
}
[Serializable]
public class Relationship
{
    public enum RelationshipType
    {
        ALLY,
        ENEMY,
        NEUTRAL
    }

    public RelationshipType Type = RelationshipType.NEUTRAL;
    [Range(0, 100f)] public float Power = 100f;
}