using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGetHit : MonoBehaviour
{
    public int life = 5;
    public CapsuleCollider2D capsColider;
    private CharacterStats stats;


    void Start()
    {
        stats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        
    }

    public void getHit()
    {
        //Pre: ---
        //Post: character received damage, if it doesn't have a shield

        if (stats.instantiatedShield != null && stats.shield.activeSelf) //s'haura de canviar un cop fet el codi de l'escut -----------------------------
        {
            
        }
    }

}
