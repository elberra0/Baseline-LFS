using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Infantry,
    Archer,
    Cavalry,
    Hero
}

public enum SimulationTurnPhase
{
    None,
    Team1Move, Team1Attack,
    Team2Move, Team2Attack,
    Ended,
}


public class Unit : MonoBehaviour
{
    #region Unit Data 
    public UnitType UnitType;
    public float UnitHP = 0.0f;
    public float UnitDamage = 0.0f;
    public int UnitVelocity = 0;
    public int UnitDisttanceToAtackTarget = 0;
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
                UnitDisttanceToAtackTarget = unitStats.InfantryDistanceToAttackTarget;
                break;

            case UnitType.Archer:
                UnitHP = unitStats.ArcherHP;
                UnitDamage = unitStats.ArcherDamage;
                UnitVelocity = unitStats.ArcherVelocity;
                UnitType = UnitType.Archer;
                UnitDisttanceToAtackTarget = unitStats.ArcherDistanceToAttackTarget;
                break;

            case UnitType.Cavalry:
                UnitHP = unitStats.CavalryHP;
                UnitDamage = unitStats.CavalryDamage;
                UnitVelocity = unitStats.CavalryVelocity;
                UnitType = UnitType.Cavalry;
                UnitDisttanceToAtackTarget = unitStats.CavalryDistanceToAttackTarget;
                break;

            case UnitType.Hero:
                UnitHP = unitStats.heroHP;
                UnitDamage = unitStats.heroDamage;
                UnitVelocity = unitStats.heroVelocity;
                UnitType = UnitType.Hero;
                UnitDisttanceToAtackTarget = unitStats.HeroDistanceToAttackTarget;
                break;
        }
    }
}

public class LowFidelitySimulation : MonoBehaviour
{
    public static LowFidelitySimulation Instance;
    private static System.Random random = new System.Random();

    #region Simulation 
    [Header("Simulation Data")]
    public UnitStats unitStats;
    public SimulationTurnPhase turnPhase = SimulationTurnPhase.None;
    public int currentTurn = 0;
    public string battleConfiguration = String.Empty;
    public int distanceBetweenTeams = 10;
    public List<List<Unit>> Team1 = new List<List<Unit>>();
    public List<List<Unit>> Team2 = new List<List<Unit>>();
    #endregion

    private Coroutine turnCoroutine;
    private int Team1TotalHP = 0;
    private int Team2TotalHP = 0;
    private bool isSimulationRunning = false;
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
        //for (int i = 0; i < Team1.Count; i++)
        //{
        //    Debug.Log($"Sublista {i}, unidades: {Team1[i].Count}");
        //
        //    for (int j = 0; j < Team1[i].Count; j++)
        //    {
        //        Unit u = Team1[i][j];
        //        Debug.Log($"[{i},{j}] Type={u.UnitType}, HP={u.UnitHP}, DMG={u.UnitDamage}, VEL={u.UnitVelocity}");
        //    }
        //}
        //
        //for (int i = 0; i < Team2.Count; i++)
        //{
        //    Debug.Log($"Sublista {i}, unidades: {Team2[i].Count}");
        //
        //    for (int j = 0; j < Team2[i].Count; j++)
        //    {
        //        Unit u = Team2[i][j];
        //        Debug.Log($"[{i},{j}] Type={u.UnitType}, HP={u.UnitHP}, DMG={u.UnitDamage}, VEL={u.UnitVelocity}");
        //    }
        //}
    }

    private void Update()
    {
        if (turnPhase == SimulationTurnPhase.None || turnPhase == SimulationTurnPhase.Ended)
        {
            if (turnCoroutine != null)
            {
                StopCoroutine(turnCoroutine);
                turnCoroutine = null;
            }

            ResetSimulation();
            turnCoroutine = StartCoroutine(TurnSystemCoroutine());
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

    private void ChangeSimulationState()
    {
        if (Team1.Count == 0 || Team2.Count == 0 || /*distanceBetweenTeams <= 0 || */ Team1TotalHP <= 0 || Team2TotalHP <= 0)
        {
            turnPhase = SimulationTurnPhase.Ended;
            Debug.LogError("Simulation Ended");

        }
    }

    private int GetTeamTotalHP(List<List<Unit>> Team)
    {
        int totalHP = 0;
        foreach (var unitGroup in Team)
        {
            foreach (var unit in unitGroup)
            {
                totalHP += (int)unit.UnitHP;
            }
        }
        return totalHP;
    }

    private int GetTeamTotalDamage(List<List<Unit>> Team, float distanceBetweenTeams)
    {
        int totalDamage = 0;

        foreach (var unitGroup in Team)
        {
            if (unitGroup.Count > 0)
            {
                if (unitGroup[0].UnitDisttanceToAtackTarget > distanceBetweenTeams || distanceBetweenTeams <= 0)
                {
                    totalDamage += (int)(unitGroup[0].UnitDamage); //We only take one unit damage as a representation
                }
            }
        }

        if (totalDamage == 0)
        {
            Debug.Log("No units in range to attack");
        }
        else
        {
            Debug.Log("Total Damage = " + totalDamage);
        }

        return totalDamage;
    }

    private int GetSlowestUnitGroupVelocity(List<List<Unit>> Team)
    {
        int slowestVelocity = int.MaxValue;
        foreach (var unitGroup in Team)
        {
            if (unitGroup.Count > 0)
            {
                int groupVelocity = unitGroup[0].UnitVelocity;
                if (groupVelocity < slowestVelocity)
                {
                    slowestVelocity = groupVelocity;
                }
            }
        }
        return slowestVelocity;
    }

    private IEnumerator MoveTeamPhase(List<List<Unit>> team, List<List<Unit>> enemy)
    {
        turnPhase = (currentTurn % 2 == 0) ? SimulationTurnPhase.Team1Move : SimulationTurnPhase.Team2Move;
        Debug.Log("MOVEMENT PHASE");

        int slowestGroupSpeed = GetSlowestUnitGroupVelocity(team);
        //Debug.Log($"Slowestt group velocity: {slowestGroupSpeed}");

        if (distanceBetweenTeams > slowestGroupSpeed)
            distanceBetweenTeams -= slowestGroupSpeed;
        else
            distanceBetweenTeams = 0;

        Debug.Log($"Distance Between Teams: {distanceBetweenTeams}");

        yield return new WaitForSeconds(1.2f);
    }
    private IEnumerator AttackTeamPhase(List<List<Unit>> team, List<List<Unit>> enemy)
    {
        turnPhase = SimulationTurnPhase.Team1Attack;
        Debug.Log("ATTACK PHASE");

        if (currentTurn % 2 != 0)
        {
            Debug.Log("Team2 attacks Team1");
            Debug.Log($"Team1 Total HP before attack: {Team1TotalHP}");
            Team1TotalHP -= GetTeamTotalDamage(team, distanceBetweenTeams);
            Debug.Log($"Team1 Total HP after attack: {Team1TotalHP}");
        }
        else
        {
            Debug.Log("Team1 attacks Team2");
            Debug.Log($"Team2 Total HP before attack: {Team2TotalHP}");
            Team2TotalHP -= GetTeamTotalDamage(team, distanceBetweenTeams);
            Debug.Log($"Team2 Total HP after attack: {Team2TotalHP}");
        }

        yield return new WaitForSeconds(1.5f);
    }

    private IEnumerator TurnSystemCoroutine()
    {
        while (turnPhase != SimulationTurnPhase.Ended)
        {
            List<List<Unit>> currentTeam = (currentTurn % 2 == 0) ? Team1 : Team2;
            List<List<Unit>> enemyTeam = (currentTurn % 2 == 0) ? Team2 : Team1;

            Debug.Log($"Turno {currentTurn}: {((currentTurn % 2 == 0) ? "Team1" : "Team2")}");

            //Move teams
            if (distanceBetweenTeams > 0)
            {
                yield return StartCoroutine(MoveTeamPhase(currentTeam, enemyTeam));
            }

            //Attack
            yield return StartCoroutine(AttackTeamPhase(currentTeam, enemyTeam));

            currentTurn++;

            ChangeSimulationState();

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ResetSimulation()
    {
        isSimulationRunning = true;
        turnPhase = SimulationTurnPhase.None;
        currentTurn = 0;
        distanceBetweenTeams = 10;

        Team1.Clear();
        Team2.Clear();

        InitializeTeamsLists(Team1);
        InitializeTeamsLists(Team2);

        string team1 = "";
        string team2 = "";

        for (int i = 0; i < 4; i++)
        {
            team1 += random.Next(4).ToString();
            team2 += random.Next(4).ToString();
        }

        CreateTeamUnits(team1, Team1);
        CreateTeamUnits(team2, Team2);

        Team1TotalHP = GetTeamTotalHP(Team1);
        Team2TotalHP = GetTeamTotalHP(Team2);

        Debug.Log("Simulation Reset - Ready to start!");
        Debug.Log("Team config " + team1 + " " + team2);
    }

}
