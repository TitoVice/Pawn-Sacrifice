using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStat : MonoBehaviour
{

    private GameObject character;
    private PopUp_Sacrifice ui;
    private string selectedStat;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => Select());
    }

    public void buttonStat(GameObject player, string stat, PopUp_Sacrifice uiToWork)
    {
        character = player;
        ui = uiToWork;
        selectedStat = stat;

        GetComponentInChildren<Text>().text = stat.ToUpper();
    }

    private void Select()
    {
        character.GetComponent<CharacterStats>().AddAbilities(selectedStat);

        foreach (Transform child in character.transform)
        {
            if (!child.CompareTag("Shield"))
            {
                foreach (Transform grandchild in child)
                {
                    if (grandchild.GetComponent<WeaponAttack>() != null) 
                    {  
                        grandchild.GetComponent<WeaponAttack>().AddProjectileStat(selectedStat);
                        break;
                    }
                }
            }
        }
        ui.Close();
    }
}
