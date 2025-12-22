using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            FindObjectOfType<CoinManager>()?.AddCoin(1);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Gem"))
        {
            FindObjectOfType<GemManager>()?.AddGem(1);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Potion"))
        {
            FindObjectOfType<PotionManager>()?.UsePotion();
            Destroy(other.gameObject);
        }
    }
}
