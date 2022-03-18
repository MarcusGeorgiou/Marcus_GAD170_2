using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighters : MonoBehaviour
{
    public string charName;
    public int charHP = 100;
    public int charATK;
    public int charDEF;
    public int charSPD;

    public GameObject nameTag;

    // Awake is run on object spawn
    void Awake()
    {
        InitStats();
    }

    private void InitStats()
    {
        charATK = Random.Range(5, 20);
        charDEF = Random.Range(5, 20);
        charSPD = Random.Range(10, 30);
    }

    public void UpdateName(string newName)
    {
        charName = newName;
        nameTag.GetComponent<TextMesh>().text = charName;
    }
}
