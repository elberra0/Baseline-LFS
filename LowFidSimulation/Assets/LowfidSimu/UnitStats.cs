using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
public class UnitStats : ScriptableObject
{
    [Header("Infantry")]
    public float InfantryHP = 2f;
    public float InfantryDamage = 1f;
    public float InfantryVelocity = 10f;

    [Header("Archer")]
    public float ArcherHP = 2f;
    public float ArcherDamage = 1f;
    public float ArcherVelocity = 10f;

    [Header("Cavalry")]
    public float CavalryHP = 4f;
    public float CavalryDamage = 2f;
    public float CavalryVelocity = 20f;

    [Header("Hero")]
    public float heroHP = 6f;
    public float heroDamage = 4f;
    public float heroVelocity = 5f;
}
