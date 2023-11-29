using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LeagueStandings : MonoBehaviour
{
    public TextMeshProUGUI standingsText;

    public class TeamData
    {
        public string teamName;
        public int gamesPlayed;
        public int wins;
        public int draws;
        public int losses;
        public int points;

    }

    private List<TeamData> teams;

    private void Start()
    {
        InitializeTeams();

        DisplayStandings();
    }

    private void InitializeTeams()
    {
        teams = new List<TeamData>();

        AddTeam("Unicorns");
        AddTeam("Ponies");
        AddTeam("Quakes");
        AddTeam("The Edmins");
        AddTeam("LMU Lions");
        AddTeam("Mariners");
    }

    private void AddTeam(string teamName)
    {
        TeamData newTeam = new()
        {
            teamName = teamName,
            gamesPlayed = 0,
            wins = 0,
            draws = 0,
            losses = 0,
            points = 0
        };

        teams.Add(newTeam);
    }

    private void DisplayStandings()
    {
        SortStandings();
        string standingsString = "Team\t\tGP\tW\tD\tL\tP\n";

        foreach (TeamData team in teams)
        {
            standingsString += $"{team.teamName}\t{team.gamesPlayed}\t{team.wins}\t{team.draws}\t{team.losses}\t{team.points}\n";
        }

        standingsText.text = standingsString;
    }

    private void SortStandings()
    {
        teams.Sort((a, b) => b.points.CompareTo(a.points));
    }
}

