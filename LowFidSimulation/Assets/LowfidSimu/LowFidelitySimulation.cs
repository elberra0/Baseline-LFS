using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Infantry,
    Archer,
    Cavalry,
    Hero
}

public class Unit : MonoBehaviour
{
    #region Unit Data 
    public UnitType UnitType;
    public float UnitHP = 0.0f;
    public float UnitDamage = 0.0f;
    public float UnitVelocity = 0.0f;
    #endregion

    private UnitStats unitStats = LowFidelitySimulation.Instance.unitStats;

    public Unit(UnitType type)
    {
        switch (type)
        {
            case UnitType.Infantry:
                UnitHP = unitStats.InfantryHP;
                UnitDamage = unitStats.InfantryDamage;
                UnitVelocity = unitStats.InfantryVelocity;
                UnitType = UnitType.Infantry;
                break;

            case UnitType.Archer:
                UnitHP = unitStats.ArcherHP;
                UnitDamage = unitStats.ArcherDamage;
                UnitVelocity = unitStats.ArcherVelocity;
                UnitType = UnitType.Archer;
                break;

            case UnitType.Cavalry:
                UnitHP = unitStats.CavalryHP;
                UnitDamage = unitStats.CavalryDamage;
                UnitVelocity = unitStats.ArcherVelocity;
                UnitType = UnitType.Cavalry;
                break;

            case UnitType.Hero:
                UnitHP = unitStats.heroHP;
                UnitDamage = unitStats.heroDamage;
                UnitVelocity = unitStats.heroVelocity;
                UnitType = UnitType.Hero;
                break;
        }
    }
}

public class LowFidelitySimulation : MonoBehaviour
{
    public static LowFidelitySimulation Instance;

    #region Simulation 
    [Header("Simulation Data")]
    public UnitStats unitStats;
    public string battleConfiguration = String.Empty;

    public List<List<Unit>> Team1 = new List<List<Unit>>();
    public List<List<Unit>> Team2 = new List<List<Unit>>();
    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        InitializeTeamsLists(Team1);
        InitializeTeamsLists(Team2);

        CreateTeamUnits("1234", Team1);
        CreateTeamUnits("4321", Team2);

        for (int i = 0; i < Team1.Count; i++)
        {
            Debug.Log($"Sublista {i}, unidades: {Team1[i].Count}");

            for (int j = 0; j < Team1[i].Count; j++)
            {
                Unit u = Team1[i][j];
                Debug.Log($"[{i},{j}] Type={u.UnitType}, HP={u.UnitHP}, DMG={u.UnitDamage}, VEL={u.UnitVelocity}");
            }
        }

        for (int i = 0; i < Team2.Count; i++)
        {
            Debug.Log($"Sublista {i}, unidades: {Team2[i].Count}");

            for (int j = 0; j < Team2[i].Count; j++)
            {
                Unit u = Team2[i][j];
                Debug.Log($"[{i},{j}] Type={u.UnitType}, HP={u.UnitHP}, DMG={u.UnitDamage}, VEL={u.UnitVelocity}");
            }
        }
    }

    private void InitializeTeamsLists(List<List<Unit>> Team)
    {
        for (int i = 0; i < 4; i++)
        {
            Team.Add(new List<Unit>());
        }
    }

    private void CreateTeamUnits(string sideTeam, List<List<Unit>> Team)
    {
        //Infantry
        for (int i = 0; i < (int)char.GetNumericValue(sideTeam[0]); i++)
        {
            Unit infantry = new Unit(UnitType.Infantry);
            Team[0].Add(infantry);
        }

        //Archer
        for (int i = 0; i < (int)char.GetNumericValue(sideTeam[1]); i++)
        {
            Unit archer = new Unit(UnitType.Archer);
            Team[1].Add(archer);
        }

        //Cavalry
        for (int i = 0; i < (int)char.GetNumericValue(sideTeam[2]); i++)
        {
            Unit cavalry = new Unit(UnitType.Cavalry);
            Team[2].Add(cavalry);
        }

        //Hero
        for (int i = 0; i < (int)char.GetNumericValue(sideTeam[3]); i++)
        {
            Unit hero = new Unit(UnitType.Hero);
            Team[3].Add(hero);
        }
    }

}
