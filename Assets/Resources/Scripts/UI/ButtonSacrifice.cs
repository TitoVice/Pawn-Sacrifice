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
        string perk1 = "";
        string perk2 = "";

        foreach (Transform child in character.transform) //projectiles modifications
        {
            if (!child.CompareTag("Shield"))
            {
                foreach (Transform grandchild in child)
                {
                    if (grandchild.GetComponent<WeaponAttack>() != null) 
                    {  
                        WeaponAttack stats = grandchild.GetComponent<WeaponAttack>();
                        foreach (KeyValuePair<string, bool> page in stats.projectileStats)
                        {
                            if (page.Value) 
                            { 
                                if (perk1 != "") { perk2 = page.Key; }
                                else { perk1 = page.Key; } 
                            }
                        }
                    }
                }
            }
        }
        if (perk2 == "") //player abilities
        {
            Dictionary<string, bool> stats = character.GetComponent<CharacterStats>().playerAbilities;
            foreach (KeyValuePair<string, bool> page in stats)
            {
                if (page.Value) 
                    { 
                        if (perk1 != "") { perk2 = page.Key; }
                        else { perk1 = page.Key; } 
                    }
            }
        }

        team.Reorganize(character);
        ui.getStats(perk1, perk2);
        //ui.Close();
    }
}
