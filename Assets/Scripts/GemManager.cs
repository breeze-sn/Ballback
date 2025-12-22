using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemManager : MonoBehaviour
{
    public int gemCount;
    public Text gemText;

    [Header("Audio")]
    public AudioClip gemSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        gemText.text = gemCount.ToString();
    }

    // ✅ ADD THIS METHOD
    public void AddGem(int amount)
    {
        gemCount += amount;

        if (audioSource != null && gemSound != null)
            audioSource.PlayOneShot(gemSound);
    }
}
