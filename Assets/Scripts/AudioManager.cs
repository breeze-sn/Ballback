using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Collect Sounds")]
    public AudioClip coinSound;
    public AudioClip gemSound;
    public AudioClip potionSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCoin()
    {
        audioSource.PlayOneShot(coinSound);
    }

    public void PlayGem()
    {
        audioSource.PlayOneShot(gemSound);
    }

    public void PlayPotion()
    {
        audioSource.PlayOneShot(potionSound);
    }
}
