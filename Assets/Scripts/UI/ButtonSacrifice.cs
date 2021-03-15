using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSacrifice : MonoBehaviour
{

    private GameObject character;
    private PopUp_Sacrifice ui;
    private TeamWorldInteraction team;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => Sacrifice());
    }

    public void buttonSacrifice(GameObject charact, TeamWorldInteraction teamToNotificate, PopUp_Sacrifice uiToWork)
    {
        character = charact;
        team = teamToNotificate;
        ui = uiToWork;

        GetComponentInChildren<Text>().text = character.GetComponent<AgentScript>().characterName;
    }

    private void Sacrifice()
    {
        //Destroy(character);
        team.Reorganize(character);
        ui.Close();
    }
}
