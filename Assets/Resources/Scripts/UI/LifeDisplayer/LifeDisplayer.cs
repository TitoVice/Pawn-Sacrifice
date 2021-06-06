using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDisplayer : MonoBehaviour
{
    private bool hasChange = false;
    private int lifes;
    private CharacterGetHit characterGetHit;
    private List<Texture2D> drawList;
    public Texture2D heart, icon;
    public RectTransform rect;

    public void Refresh(GameObject character, Texture2D texture)
    {
        //Pre: ---
        //Post: refresh the character

        icon = texture;
        foreach (Transform child in character.transform)
        {
            if (child.CompareTag("HitDetector")) { characterGetHit = child.GetComponent<CharacterGetHit>(); lifes = characterGetHit.life; }
        }
        hasChange = true;
        Connect();
    }

    public void ChangeLife(int changed)
    {
        //Pre: ---
        //Post: change the life +-changed

        lifes += changed;
        hasChange = true;
    }

    private void Connect()
    {
        characterGetHit.GetDisplayer(this);
    }

    public void Delete()
    {
        //Pre: ---
        //Post: destroys itself

        Destroy(gameObject);
    }

    void OnGUI()
    {
        if (hasChange)
        {
            //Rect posRect = new Rect(rect.transform.position.x, rect.transform.position.y, heart.width / 5 * lifes, heart.height);
            //Rect texRect = new Rect(0,0,1.0f / 5 * lifes, 1.0f);
            Rect posRect = new Rect(transform.position.x, transform.position.y, icon.width, icon.height);
            int width;

            Rect textureIcon = new Rect(10, 0, icon.width, icon.height); //for the icon
            width = icon.width + 20;
            GUI.DrawTextureWithTexCoords(posRect, icon, textureIcon);

            for (int i = 0; i < lifes; i++)
            {
                Rect textureHeart = new Rect(width * (i+1), 0, heart.width, heart.height);
                width = heart.width;
                GUI.DrawTextureWithTexCoords(posRect, heart, textureHeart);
            }
            hasChange = false;
        }
    }
}
