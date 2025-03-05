using UnityEngine;
using TMPro;

public class HealthTextUpdater : MonoBehaviour
{
    public PlayerHealth playerHealth; // Referencia al script de salud del jugador
    private TextMeshProUGUI healthText; // Referencia al componente de texto

    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();

        if (playerHealth == null)
        {
            Debug.LogError(gameObject.name + " no tiene un PlayerHealth asignado.");
        }
    }

    void Update()
    {
        if (playerHealth != null)
        {
            healthText.text = playerHealth.GetCurrentHealth().ToString("F0"); // Muestra la vida sin decimales
        }
    }
}
