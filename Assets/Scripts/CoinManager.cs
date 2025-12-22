using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public Text coinText;

    [Header("Audio")]
    public AudioClip coinSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        coinText.text = coinCount.ToString();
    }

    // ✅ ADD THIS METHOD
    public void AddCoin(int amount)
    {
        coinCount += amount;

        if (audioSource != null && coinSound != null)
            audioSource.PlayOneShot(coinSound);
    }
}
