using UnityEngine;

public class ReviveBehaviour : MonoBehaviour
{
    private CharacterDeath[] characters = new CharacterDeath[4];
    public bool usedInFloor = false;

    void Start()
    {
        int i = 0;
        foreach (Transform character in transform.parent)
        {
            characters[i] = character.GetComponent<CharacterDeath>();
            i++;
        }
    }

    void Update()
    {
        if (!usedInFloor)
        {
            foreach(CharacterDeath character in characters)
            {
                if (character != null && character.isDead)
                {
                    usedInFloor = true;
                    character.Revived();
                    break;
                }
            }
        }
    }

    public void Refull()
    {
        usedInFloor = false;
    }
}
