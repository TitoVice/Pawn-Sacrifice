using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDisplayer : MonoBehaviour
{
    public int lifes = 5;
    private CharacterGetHit characterGetHit;
    private List<Texture2D> drawList;
    public Texture2D heart, icon;
    private float texWidth, texHeight;
    private float faceHeight, faceWidth, xPosFace, yPosFace;

    void Start()
    {
        texWidth = heart.width*2;
        texHeight = heart.height*2;
    }

    public void Refresh(GameObject character, Texture2D texture)
    {
        //Pre: ---
        //Post: refresh the character

        icon = texture;
        faceWidth = icon.width*1.5f;
        faceHeight = icon.height*1.5f;
        xPosFace = transform.position.x;
        yPosFace = transform.position.y;

        foreach (Transform child in character.transform)
        {
            if (child.CompareTag("HitDetector")) { characterGetHit = child.GetComponent<CharacterGetHit>(); lifes = characterGetHit.life; }
        }
        Connect();
    }

    public void ChangeLife(int changed)
    {
        //Pre: ---
        //Post: change the life +-changed

        lifes += changed;
    }

    public void FullHealth(int fullHealth)
    {
        lifes = fullHealth;
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
        
        Rect posRectFace = new Rect(xPosFace, yPosFace,faceWidth, faceHeight);
        Rect texRectFace = new Rect(0,0,1.0f, 1.0f);
        GUI.DrawTextureWithTexCoords(posRectFace, icon, texRectFace);

        if (lifes > 0) {
            
            Rect posRect = new Rect(xPosFace + 50,yPosFace, texWidth / 5 * lifes, texHeight);
            Rect texRect = new Rect(0,0,(1.0f / 5) * lifes, 1.0f);
            GUI.DrawTextureWithTexCoords(posRect, heart, texRect);
        }

    }
}
