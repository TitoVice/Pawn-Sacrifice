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
    private BoxCollider2D meleeCollider;
    private float damage;
    private float attackSpeed;

    public bool distanceAttack = false;
    public GameObject projectile;
    public GameObject player;
    public Dictionary<string, bool> projectileStats = new Dictionary<string, bool>{ {"arrow", false}, {"stun", false}, {"burn", false} };

    private bool shooted = false;
    private float timer = 0.0f;
    private bool ableToAttack = true;
    private bool going = true;
    private bool cooldown = false;
    public Transform initialPos;
    public Transform finalPos;

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

        if (GetComponent<MeleeWeaponAttack>() != null) 
        { 
            meleeCollider = GetComponent<BoxCollider2D>();
            meleeCollider.enabled = false;
            GetComponent<MeleeWeaponAttack>().getVariables(projectileStats, damage); 
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && (!shooted && ableToAttack && !cooldown) && player.CompareTag("Player"))
        {
            if (distanceAttack) { shoot(); shooted = true;}
            else { meleeAttack();  ableToAttack = false;}
        }

        if (shooted && timer < attackSpeed) 
        { 
            timer += Time.deltaTime; 
            if (timer >= attackSpeed) { shooted = false; timer = 0.0f; }
        }

        if (cooldown)
        {
            timer += Time.deltaTime; 
            if (timer >= attackSpeed) { cooldown = false; timer = 0.0f; }
        }

        if (!ableToAttack)
        {
            if (Vector3.Distance(transform.position, finalPos.position) <= 0.02f || !going) //melee returning
            {
                transform.position = Vector3.MoveTowards(transform.position, initialPos.position, Time.deltaTime*7);
                going = false;
                if (Vector3.Distance(transform.position, initialPos.position) <= 0.02f) //arrived to initial pos
                    { 
                        ableToAttack = true; 
                        going = true; 
                        transform.position = initialPos.position; 
                        cooldown = true; 
                        meleeCollider.enabled = false;
                    } 
            }
            else { transform.position = Vector3.MoveTowards(transform.position, finalPos.position, Time.deltaTime*7); } //melee going to the end
        }
    }

    private void shoot()
    {
        //Pre: ---
        //Post: instanciates the projectile and shoots it

        GameObject element = Instantiate(projectile, transform.position, transform.parent.rotation);
        element.GetComponent<ProjectileHit>().getStats(projectileStats, damage);
        element.GetComponent<ProjectileMovement>().getShotDirection((transform.position - transform.parent.position).normalized);
        
    }

    private void meleeAttack()
    {
        //Pre: angle of the direction to attack
        //Post: lets the weapon make damage

        meleeCollider.enabled = true;
    }

    public void returnMeleeWeapon()
    {
        //Pre: ---
        //Post: make the weapon to return with the player

        going = false;
    }

    public void AddProjectileStat(string type)
    {
        //Pre: ---
        //Post: adds a new projectile stat
        Debug.Log(type);
        if (projectileStats.ContainsKey(type)) 
        { 
            projectileStats[type] = true;  
            Debug.Log(projectileStats[type]);
            if (GetComponent<MeleeWeaponAttack>() != null) 
            {
                GetComponent<MeleeWeaponAttack>().updateStats(projectileStats);
            }
        }
    }
}
