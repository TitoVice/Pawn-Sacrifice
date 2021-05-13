using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAWeaponMovement : MonoBehaviour
{
    public float angle;
    private SpriteRenderer sprite;
    private EnemyMoveScript moveScript;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        moveScript = transform.parent.transform.parent.GetComponent<EnemyMoveScript>();
    }

    void Update()
    {
        Vector3 objective = moveScript.target.position;
        objective.z = 0;
        Vector3 object_pos = transform.parent.position;
        objective.x = objective.x - object_pos.x;
        objective.y = objective.y - object_pos.y;
        angle = Mathf.Atan2(objective.y, objective.x) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(transform.parent.eulerAngles.z, angle, 12 * Time.deltaTime);
        transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
