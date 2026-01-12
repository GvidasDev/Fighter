using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Abilities { 
    None,
    Shield,
    Immune,
    Speed
}

[CreateAssetMenu()]
public class PlayerAbilitiesSO : ScriptableObject
{
    public Abilities Abilitie;
}
