using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Niche")]
public class Niche : ScriptableObject
{
    public GameObject NicheOccupant;
    public float MinX, MaxX, MinZ, MaxZ, MaxElev, MinElve, ProbabilityPerHeter;
}
