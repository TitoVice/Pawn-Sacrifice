using UnityEngine;

public class BossProjectileHit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitDetector"))
        {
            CharacterGetHit character = collision.gameObject.GetComponent<CharacterGetHit>();

            character.getHit();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Background") || collision.CompareTag("Shield")) { Destroy(gameObject); }
    }
}
