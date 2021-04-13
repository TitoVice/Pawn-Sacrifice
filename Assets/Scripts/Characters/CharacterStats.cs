using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Basic Stats")]
    public float attackSpeed = 0.7f;
    public float attackDamage = 1.0f;
    public float speed = 1.0f;

    [System.Serializable]
    public struct auxDictionary{ public string name; public bool used; } //needs to have the same names as playerAbilities
    
    [Header("Special Abilities")]
    public auxDictionary[] dictionary;
    public Dictionary<string, bool> playerAbilities = new Dictionary<string, bool>{ {"vision", false}, {"revive", false}, {"backprotect", false}, {"shield", false}, {"spawner", false} };
    
    void Start()
    {
        for (int i = 0; i < dictionary.Length; i++)
        {
            if (playerAbilities.ContainsKey(dictionary[i].name))
            {
                playerAbilities[dictionary[i].name] = dictionary[i].used;
            }
        }
    }
    void Update()
    {
        
    }
}
