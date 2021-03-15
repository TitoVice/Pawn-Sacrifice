using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsInteraction : MonoBehaviour
{
    private GameObject camera;
    private RoomDoors parent;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        parent = gameObject.transform.parent.transform.parent.GetComponent<RoomDoors>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (parent.doorPassing()) { centerCamera(); }
        }
    }

    private void centerCamera()
    {
        camera.transform.position = new Vector3(parent.transform.position.x, parent.transform.position.y, -2);
    }
}
