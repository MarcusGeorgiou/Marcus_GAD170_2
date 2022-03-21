using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimBattle : MonoBehaviour
{
    public GameObject fighterPrefab;
    public int teamSize = 3;

    public string[] fighterNames;
    public GameObject[] aFighters;
    public GameObject[] bFighters;

    // Start is called before the first frame update
    void Start()
    {
        // Create teams and call generation
        aFighters = GenerateTeam(aFighters);
        bFighters = GenerateTeam(bFighters);

        // Randomly chose fighters
        GameObject randA = aFighters[Random.Range(0, teamSize)];
        GameObject randB = bFighters[Random.Range(0, teamSize)];
        Battle(randA, randB);
    }

    public GameObject[] GenerateTeam(GameObject[] incTeam)
    {
        // Create team and spawn fighters
        incTeam = new GameObject[teamSize];
        for(int i = 0; i < teamSize; i++)
        {
            // Spawn fighter (go = game object)
            GameObject go = Instantiate(fighterPrefab);

            // Asign to team
            incTeam[i] = go;

            // Pick random name for fighter
            go.GetComponent<Fighters>().UpdateName(fighterNames[Random.Range(0, fighterNames.Length)]);
        }

        return incTeam;
    }

    public void Battle(GameObject fighterA, GameObject fighterB)
    {
        Fighters fAStats = fighterA.GetComponent<Fighters>();
        Fighters fBStats = fighterB.GetComponent<Fighters>();

        if(fAStats.charSPD >= fBStats.charSPD)
        {
            fBStats.charHP -= fAStats.charATK - fBStats.charDEF;

            Debug.Log("Fighter A attacks Fighter B");
            Debug.Log("Fighter B's health is now: " + fBStats.charHP);
        }
        else
        {
            fAStats.charHP -= fBStats.charATK - fAStats.charDEF;

            Debug.Log("Fighter B attacks Fighter A");
            Debug.Log("Fighter A's health is now: " + fAStats.charHP);
        }
    }
}
