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

    public GameObject backProtect; public GameObject shield; public GameObject instantiatedShield = null;
    
    void Start()
    {
        for (int i = 0; i < dictionary.Length; i++)
        {
            if (playerAbilities.ContainsKey(dictionary[i].name))
            {
                playerAbilities[dictionary[i].name] = dictionary[i].used;
                if (dictionary[i].used) { AddAbilities(dictionary[i].name); }
            }
        }
    }

    public void AddAbilities(string ability)
    {
        //Pre: valid ability name
        //Post: add the ability to the character

        if (ability == "vision") { gameObject.AddComponent<VisionBehaviour>(); playerAbilities[ability] = true; }
        else if (ability == "revive") { gameObject.AddComponent<ReviveBehaviour>(); playerAbilities[ability] = true; }
        else if (ability == "spawner") { gameObject.AddComponent<ReviveBehaviour>(); playerAbilities[ability] = true; }
        else if (ability == "backprotect") 
        { 
            foreach (Transform child in transform){
                if (!child.CompareTag("Shield") && !child.CompareTag("HitDetector"))
                {
                    Instantiate(backProtect, child);
                    playerAbilities[ability] = true; 
                }
            }
        }
        else if (ability == "shield") { instantiatedShield = Instantiate(shield, transform); playerAbilities[ability] = true; }
    }
}
