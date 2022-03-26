using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScript : MonoBehaviour
{
    public GameState gState;
    public UI textBox;
    private string message;
    private bool simulate = true;
    public string victor;

    public GameObject fighterPrefab;
    public int teamSize = 3;

    public GameObject[] aFighters;
    public GameObject[] bFighters;
    public Material MAT_TeamA;
    public Material MAT_TeamB;

    public GameObject randA;
    public GameObject randB;

    // Define gamestates
    public enum GameState
    {
        input,
        prep,
        select,
        fight,
        result,
        victory
    }

    public void Teams()
    {
        // Create Teams
        aFighters = CreateTeam(aFighters, MAT_TeamA);
        bFighters = CreateTeam(bFighters, MAT_TeamB);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            message = "Starting...";
            Debug.Log(message);
            textBox.NewMessage(message);
            StartCoroutine(TransitionTimer(2f, GameState.prep));
        }

        // Do not allow script to run every frame
        if (simulate)
        {
            simulate = false;
            switch (gState)
            {
                case GameState.input:
                    message = "Press space to start";
                    Debug.Log(message);
                    textBox.NewMessage(message);

                    break;
                case GameState.prep:
                    message = "Preparing...";
                    Debug.Log(message);
                    textBox.NewMessage(message);

                    Teams();
                    StartCoroutine(TransitionTimer(2f, GameState.select));

                    break;
                case GameState.select:
                    // Randomly choose fighters
                    randA = aFighters[Random.Range(0, teamSize)];
                    randB = bFighters[Random.Range(0, teamSize)];

                    if(randA.GetComponent<Fighters>().charHP >= 0 && randB.GetComponent<Fighters>().charHP >= 0)
                    {
                        message = "Picking fighters...";
                        Debug.Log(message);
                        textBox.NewMessage(message);

                        StartCoroutine(TransitionTimer(2f, GameState.fight));
                    }
                    else
                    {
                        StartCoroutine(TransitionTimer(0f, GameState.select));
                    }

                    break;
                case GameState.fight:
                    Battle(randA, randB);
                    TeamCheck();

                    break;
                case GameState.result:
                    message = "The winner is...";
                    Debug.Log(message);
                    textBox.NewMessage(message);

                    WinningTeam();

                    break;
                case GameState.victory:
                    message = "GAME END!!!";
                    Debug.Log(message);
                    textBox.NewMessage(message);

                    break;
            }
        }
    }

    public GameObject[] CreateTeam(GameObject[] incTeam, Material teamMat)
    {
        // Create and spawn teams
        incTeam = new GameObject[teamSize];

        for(int i = 0; i < teamSize; i++)
        {
            // Spawn fighters at random location
            float randX = Random.Range(-18, 18);
            float randZ = Random.Range(0, 10);
            Vector3 pos = new Vector3(transform.position.x + randX,
                                      transform.position.y,
                                      transform.position.z + randZ);

            GameObject go = Instantiate(fighterPrefab, pos, transform.rotation);
            go.GetComponent<MeshRenderer>().material = teamMat;

            // Assign to team
            incTeam[i] = go;

        }

        return incTeam;
    }

    public void Battle(GameObject fighterA, GameObject fighterB)
    {
        Fighters fAStats = fighterA.GetComponent<Fighters>();
        Fighters fBStats = fighterB.GetComponent<Fighters>();

        if (fAStats.charSPD >= fBStats.charSPD)
        {
            int dmg = fAStats.charATK - fBStats.charDEF;
            dmg = Mathf.Clamp(dmg, 1, fAStats.charATK);
            fBStats.charHP -= dmg;

            Debug.Log("Fighter A attacks Fighter B");
            Debug.Log("Fighter B's health is now: " + fBStats.charHP);
            message = "Fighter B has " + fBStats.charHP + "hp";
            Debug.Log(message);
            Debug.Log(dmg);
            textBox.NewMessage(message);

            AStart(fighterA, fighterB);
        }
        else
        {
            int dmg = fBStats.charATK - fAStats.charDEF;
            dmg = Mathf.Clamp(dmg, 1, fBStats.charATK);
            fAStats.charHP -= dmg;

            Debug.Log("Fighter B attacks Fighter A");
            Debug.Log("Fighter A's health is now: " + fAStats.charHP);
            message = "Fighter A has " + fAStats.charHP + "hp";
            Debug.Log(message);
            Debug.Log(dmg);
            textBox.NewMessage(message);

            BStart(fighterA, fighterB);
        }

        if (fAStats.charHP > 0 && fBStats.charHP > 0)
        {
            StartCoroutine(TransitionTimer(3f, GameState.fight));
        }
        else
        {
            StartCoroutine(TransitionTimer(2f, GameState.select));
        }
    }

    public void AStart(GameObject fighterA, GameObject fighterB)
    {
        // A has attacked first so B must now go
        Fighters fAStats = fighterA.GetComponent<Fighters>();
        Fighters fBStats = fighterB.GetComponent<Fighters>();

        int dmg = fBStats.charATK - fAStats.charDEF;
        dmg = Mathf.Clamp(dmg, 1, fBStats.charATK);
        fAStats.charHP -= dmg;

        Debug.Log("Fighter B attacks Fighter A");
        Debug.Log("Fighter A's health is now: " + fAStats.charHP);
        message = "Fighter A has " + fAStats.charHP + "hp";
        Debug.Log(message);
        Debug.Log(dmg);
        textBox.NewMessage(message);
    }

    public void BStart(GameObject fighterA, GameObject fighterB)
    {
        // B has attacked first so A must now go
        Fighters fAStats = fighterA.GetComponent<Fighters>();
        Fighters fBStats = fighterB.GetComponent<Fighters>();

        int dmg = fAStats.charATK - fBStats.charDEF;
        dmg = Mathf.Clamp(dmg, 1, fAStats.charATK);
        fBStats.charHP -= dmg;

        Debug.Log("Fighter A attacks Fighter B");
        Debug.Log("Fighter B's health is now: " + fBStats.charHP);
        message = "Fighter B has " + fBStats.charHP + "hp";
        Debug.Log(message);
        Debug.Log(dmg);
        textBox.NewMessage(message);
    }

    public void TeamCheck()
    {

        int killed = 0;
        for (int i = 0; i < teamSize; i++)
        {
            if (aFighters[i].GetComponent<Fighters>().charHP <= 0)
            {
                killed++;
            }
            if (killed >= teamSize)
            {
                Debug.Log("Team A is defeated!");
                victor = "Team B";
                StartCoroutine(TransitionTimer(2f, GameState.result));
            }
        }
        killed = 0;
        for (int x = 0; x < teamSize; x++)
        {
            if (bFighters[x].GetComponent<Fighters>().charHP <= 0)
            {
                //character is dead
                killed++;
            }
            if (killed >= teamSize)
            {
                Debug.Log("Team B is defeated!");
                victor = "Team A";
                StartCoroutine(TransitionTimer(2f, GameState.result));
            }
        }
    }

    public void WinningTeam()
    {
        message = victor;
        Debug.Log(message);
        textBox.NewMessage(message);

        StartCoroutine(TransitionTimer(2f, GameState.victory));
    }

    IEnumerator TransitionTimer(float delay, GameState newState)
    {
        yield return new WaitForSeconds(delay);
        gState = newState;
        simulate = true;
    }
}
