using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public GameObject team;
    public List<LifeDisplayer> displayers;
    private int maxPlayers = 4;
    [System.Serializable]
    public struct auxDictionary{ public string name; public Texture2D icon; } //needs to have the same names as playerAbilities
    
    [Header("Icons")]
    public auxDictionary[] dictionary;
    private Dictionary<string, Texture2D> lifeDictionary = new Dictionary<string, Texture2D>();

    void Start()
    {
        for (int i = 0; i < dictionary.Length; i++)
        {
            lifeDictionary.Add(dictionary[i].name, dictionary[i].icon);
        }
    }

    public void RefreshLifes()
    {
        //Pre: ---
        //Post: refreshes every displayer with the necessary character or deletes the displayer

        int i = 0;
        foreach (Transform character in team.transform)
        {
            displayers[i].Refresh(character.gameObject, lifeDictionary[character.name]);
            i++;
        }

        for(int j = i; j < maxPlayers; j++)
        {
            displayers[j].Delete();
            displayers.RemoveAt(j);
            maxPlayers--;
        }
    }
}
