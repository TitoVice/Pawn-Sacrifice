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

        GameObject team = GameObject.Find("Team");
        foreach (Transform character in team.transform)
        {
            print(character.name);
            foreach (Transform child in character)
            {
                print(child.name);
                if (child.CompareTag("HitDetector")) { child.GetComponent<CharacterGetHit>().CureHealth(); break; }
            }
            
            if (character.GetComponent<ReviveBehaviour>()) { character.GetComponent<ReviveBehaviour>().Refull(); } //revive ability
            if (character.GetComponent<ShieldBehaviour>()) { character.GetComponent<ShieldBehaviour>().Activate(); } //revive ability
        }
    }

}
