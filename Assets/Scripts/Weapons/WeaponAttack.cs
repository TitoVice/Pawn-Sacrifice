using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    [System.Serializable]
    public struct auxDictionary{ public string name; public bool used; } //needs to have the same names as projectileStats
    public auxDictionary[] dictionary;

    private CharacterStats playerStats;
    private WeaponMovement movement;
    private float damage;
    private float attackSpeed;

    public bool distanceAttack = false;
    public GameObject projectile;
    public Dictionary<string, bool> projectileStats = new Dictionary<string, bool>{ {"arrow", false}, {"stun", false}, {"burn", false} };

    void Start()
    {
        playerStats = transform.parent.transform.parent.GetComponent<CharacterStats>();
        movement = GetComponent<WeaponMovement>();
        damage = playerStats.attackDamage;
        attackSpeed = playerStats.attackSpeed;

        for (int i = 0; i < dictionary.Length; i++)
        {
            if (projectileStats.ContainsKey(dictionary[i].name))
            {
                projectileStats[dictionary[i].name] = dictionary[i].used;
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (distanceAttack) { shoot(); }
            else { meleeAttack(); }
        }
    }

    private void shoot()
    {
        //Pre: ---
        //Post: instanciates the projectile and shoots it

        GameObject element = Instantiate(projectile, transform.position, transform.parent.rotation);
        element.GetComponent<ProjectileHit>().getStats(projectileStats);
        element.GetComponent<ProjectileMovement>().getShotDirection((transform.position - transform.parent.position).normalized);
        
    }

    private void meleeAttack()
    {
        //Pre: angle of the direction to attack
        //Post: does a melee attack

    }
}
