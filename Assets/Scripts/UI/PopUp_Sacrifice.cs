using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp_Sacrifice : MonoBehaviour
{
    public GameObject ui;
    public GameObject team;
    public GameObject buttonPrefab;

    private Transform panel;
    private GameObject[] elections;
    private GameObject[] buttons;

    private void Start()
    {
        panel = ui.transform.Find("Panel");
    }

    public void Open()
    {
        ui.SetActive(true);
        
        int childCount = team.transform.childCount;
        buttons = new GameObject[childCount - 1]; //-1 cause the main character cannot be deleted
        elections = new GameObject[childCount];

        int j = 0; //for the buttons
        for (int i = 0; i < childCount; i++)
        {
            elections[i] = team.transform.GetChild(i).gameObject;

            if (!elections[i].CompareTag("Player"))
            {
                buttons[j] = Instantiate(buttonPrefab, panel);
                buttons[j].GetComponent<ButtonSacrifice>().buttonSacrifice(elections[i], team.GetComponent<TeamWorldInteraction>(), GetComponent<PopUp_Sacrifice>());
                j++;
            }
        }
        stopCharacters();

        if (buttons.Length == 0) { Close(); }
        else { fitButtons(); }
    }

    public void Close()
    {
        ui.SetActive(false);

        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i]);
        }

        moveCharacters();

        GameObject end = GameObject.FindGameObjectsWithTag("End")[0];
        end.GetComponent<NextLevel>().passLevel();
    }

    private void stopCharacters()
    {
        for (int i = 0; i < elections.Length; i++)
        {
            if (elections[i].CompareTag("Player"))
            {
                elections[i].GetComponent<PlayerMovement>().Immobilize();
            }
            else
            {
                elections[i].GetComponent<AgentScript>().Immobilize();
            }
        }
    }
    private void moveCharacters()
    {
        for (int i = 0; i < elections.Length; i++)
        {
            if (elections[i].CompareTag("Player"))
            {
                elections[i].GetComponent<PlayerMovement>().Mobilize();
            }
            else
            {
                elections[i].GetComponent<AgentScript>().Mobilize();
            }
        }
    }

    private void fitButtons()
    {
        float panelWidth = panel.GetComponent<RectTransform>().rect.width;
        float panelHeight = panel.GetComponent<RectTransform>().rect.height;
        HorizontalLayoutGroup layout = panel.gameObject.GetComponent<HorizontalLayoutGroup>();
        RectOffset auxPadding = new RectOffset(layout.padding.left, layout.padding.right, layout.padding.top, layout.padding.bottom );

        auxPadding.top = Mathf.RoundToInt(panelHeight / 3.5f);
        auxPadding.bottom = Mathf.RoundToInt(panelHeight / 3.5f);
        auxPadding.left = Mathf.RoundToInt(panelWidth / 6);
        auxPadding.right = Mathf.RoundToInt(panelWidth / 6);

        layout.padding = auxPadding;
    }
}
