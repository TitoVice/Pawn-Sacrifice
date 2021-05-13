using UnityEngine;

public class ReviveBehaviour : MonoBehaviour
{
    private CharacterGetHit[] characters = new CharacterGetHit[4];
    private bool usedInFloor = false;

    void Start()
    {
        int i = 0;
        foreach (Transform character in transform.parent.transform)
        {
            characters[i] = character.GetComponent<CharacterGetHit>();
            i++;
        }
    }

    void Update()
    {
        if (!usedInFloor)
        {
            foreach(CharacterGetHit character in characters)
            {
                if (character != null && character.dead)
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
