using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    private GameObject gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameController");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameController.GetComponent<PopUp_Sacrifice>().Open();
        }
        
    }

    public void passLevel()
    {
        transform.parent.GetComponent<FloorGenerator>().Create();
    }

}
