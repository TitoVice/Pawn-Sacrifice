using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    public float angle;

    private SpriteRenderer sprite;
    private bool isPlayer = false;
    public AgentScript agentScript = null;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        isPlayer = transform.parent.transform.parent.CompareTag("Player");
    }

    void Update()
    {
        Vector3 target = Vector3.forward;

        if (isPlayer)
        {
            target = Input.mousePosition;
        }
        else
        {
            target = agentScript.giveTarget();
        }

        if (target != null) { weaponMoves(target); }
    }

    private void weaponMoves(Vector3 target)
    {
        //Pre: ---
        //Post: move weapon towards the target

        target.z = 0;
        Vector3 object_pos;
        if (isPlayer) { object_pos = Camera.main.WorldToScreenPoint(transform.parent.position); }
        else { object_pos = transform.parent.position; }

        target.x = target.x - object_pos.x;
        target.y = target.y - object_pos.y;
        angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(transform.parent.eulerAngles.z, angle, 12 * Time.deltaTime);
        transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    
}