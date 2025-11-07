using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Unit : MonoBehaviour {

    [field: SerializeField] public Fraction Fraction { get; protected set; }
    [field: SerializeField] public Health Health { get; protected set; }
    [field: SerializeField] public List<Skill> Skills { get; protected set; }

    protected virtual void Awake() {
        Health = GetComponent<Health>();
    }

    public void Build(Fraction fraction) {
        Awake();
        Fraction = fraction;
    }

}