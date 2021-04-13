using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    public float angle;

    void Update()
    {
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 0;
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.parent.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(transform.parent.eulerAngles.z, angle, 12 * Time.deltaTime);
        transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}