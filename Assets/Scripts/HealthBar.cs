using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar Configuration")]
    public Slider healthSlider;
    public Text healthText; // Texte optionnel pour afficher la santé en chiffres

    public void SetSlider(float currentHealth)
    {
        if (healthSlider == null) return;

        healthSlider.value = currentHealth; // Mise à jour directe de la valeur

        // Mettre à jour le texte si disponible
        if (healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(healthSlider.maxValue)}";
        }
    }

    public void SetSliderMax(float maxHealth)
    {
        if (healthSlider == null) return;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; // Initialiser à pleine santé

        // Mettre à jour le texte si disponible
        if (healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(maxHealth)}/{Mathf.RoundToInt(maxHealth)}";
        }
    }
}