using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoin = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true; // sudah terambil, supaya tidak terambil 2x
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
            gameObject.SetActive(false); // menonaktifkan jika sudah terambil
            Destroy(gameObject);
            FindAnyObjectByType<GameSession>().AddScorePlayer(pointsForCoin);
        }
    }
}