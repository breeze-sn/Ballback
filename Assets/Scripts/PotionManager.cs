using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    [Tooltip("Percentage of max health restored")]
    [Range(0f, 1f)]
    public float healPercent = 0.2f;

    [Header("Audio")]
    public AudioClip potionSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // ✅ ADD THIS METHOD
    public void UsePotion()
    {
        if (audioSource != null && potionSound != null)
            audioSource.PlayOneShot(potionSound);

        // Healing logic can be added later
    }
}
