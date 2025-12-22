using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI References")]
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    public void SetMaxHealth(int health)
    {
        if (slider == null || fill == null || gradient == null)
        {
            Debug.LogError("HealthBar references not assigned!");
            return;
        }

        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        if (slider == null || fill == null || gradient == null)
            return;

        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
