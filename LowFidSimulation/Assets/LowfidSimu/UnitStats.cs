using NUnit.Framework.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
public class UnitStats : ScriptableObject
{
    [Header("Infantry")]
    public float InfantryHP = 2f;
    public float InfantryDamage = 1f;
    public int InfantryVelocity = 2;
    public int InfantryDistanceToAttackTarget = 1;

    [Header("Archer")]
    public float ArcherHP = 2f;
    public float ArcherDamage = 1f;
    public int ArcherVelocity = 2;
    public int ArcherDistanceToAttackTarget = 5;

    [Header("Cavalry")]
    public float CavalryHP = 4f;
    public float CavalryDamage = 2f;
    public int CavalryVelocity = 5;
    public int CavalryDistanceToAttackTarget = 1;


    [Header("Hero")]
    public float heroHP = 6f;
    public float heroDamage = 4f;
    public int heroVelocity = 1;
    public int HeroDistanceToAttackTarget = 1;
}
