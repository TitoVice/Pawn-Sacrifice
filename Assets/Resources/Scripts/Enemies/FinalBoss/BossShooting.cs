using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject projectile;

    public void Shoot()
    {
        //Pre: ---
        //Post: shoot fireballs

        Vector3 direction = new Vector3(transform.position.x+1, transform.position.y, transform.position.z) - transform.position;
        int randomAngle = Random.Range(0, 45);
        float rotationDegrees = randomAngle;
        float rotationRads = randomAngle*Mathf.Deg2Rad;

        for (int i = 0; i < 8; i++)
        {
            Vector3 newDirection = new Vector3(Mathf.Cos(rotationRads)*direction.x - Mathf.Sin(rotationRads)*direction.y, Mathf.Sin(rotationRads)*direction.x + Mathf.Cos(rotationRads)*direction.y, direction.z);

            shootProjectile(newDirection.normalized, rotationDegrees);
            rotationRads += Mathf.PI/4;
            rotationDegrees += 45;
        }
    }

    private void shootProjectile(Vector3 direction, float rotation)
    {
        //Pre: ---
        //Post: instanciates the projectile and shoots it

        GameObject element = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, rotation)));
        element.GetComponent<ProjectileMovement>().getShotDirection(direction);
        
    }
}
