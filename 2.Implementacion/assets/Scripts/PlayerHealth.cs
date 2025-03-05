using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject winnerMessage; // Referencia al GameObject WinnerMessage
    public GameObject endGameMessages; // Referencia al Canvas EndGameMessages

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log(gameObject.name + " ha iniciado con " + currentHealth + " de vida.");
    }

public void TakeDamage(float damage)
{
    // Verifica si el jugador está defendiendo
    PlayerLeftController leftController = GetComponent<PlayerLeftController>();
    PlayerRightController rightController = GetComponent<PlayerRightController>();

    bool isDefending = false;

    // Comprueba si el jugador es izquierdo o derecho y si está defendiendo
    if (leftController != null)
    {
        isDefending = leftController.isDefending;
    }
    else if (rightController != null)
    {
        isDefending = rightController.isDefending;
    }

    // Si no está defendiendo, aplica el daño y el retroceso
    if (!isDefending)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " recibió " + damage + " de daño. Vida restante: " + currentHealth);

        // Aplica retroceso
        if (leftController != null)
        {
            // Jugador izquierdo retrocede hacia la izquierda
            transform.Translate(Vector2.left * 0.5f); // Ajusta el valor 0.5f según el retroceso deseado
        }
        else if (rightController != null)
        {
            // Jugador derecho retrocede hacia la derecha
            transform.Translate(Vector2.right * 0.5f); // Ajusta el valor 0.5f según el retroceso deseado
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    else
    {
        currentHealth -= 1; // Aplica un daño reducido si está defendiendo
        Debug.Log(gameObject.name + " bloqueó el ataque y recibió 1 de daño. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}

void Die()
{
    Debug.Log(gameObject.name + " ha sido derrotado.");

    // Activa el mensaje de victoria
    if (winnerMessage != null)
    {
        winnerMessage.SetActive(true); // Muestra el mensaje

        // Cambia el texto según el jugador derrotado
        TextMeshProUGUI winnerText = winnerMessage.GetComponentInChildren<TextMeshProUGUI>();
        if (gameObject.name.Contains("Left")) // Si el jugador izquierdo muere
        {
            winnerText.text = "Ganador: Jugador 2";
        }
        else if (gameObject.name.Contains("Right")) // Si el jugador derecho muere
        {
            winnerText.text = "Ganador: Jugador 1";
        }
    }
    else
    {
        Debug.LogError("winnerMessage no está asignado en el Inspector.");
    }

    // Activa el Canvas EndGameMessages
    if (endGameMessages != null)
    {
        endGameMessages.SetActive(true); // Muestra los botones
    }
    else
    {
        Debug.LogError("endGameMessages no está asignado en el Inspector.");
    }

    // Desactiva los scripts de movimiento y ataque de ambos jugadores
    PlayerLeftController leftController = GameObject.Find("PlayerLeft").GetComponent<PlayerLeftController>();
    PlayerRightController rightController = GameObject.Find("PlayerRight").GetComponent<PlayerRightController>();

    if (leftController != null) leftController.enabled = false;
    if (rightController != null) rightController.enabled = false;
}

/* Método Die antes de agregar el Canva con los botones de reinicio 
void Die()
{
    Debug.Log(gameObject.name + " ha sido derrotado.");
//    gameObject.SetActive(false); // Desactiva el jugador cuando muere

    // Activa el mensaje de victoria
    if (winnerMessage != null)
    {
        winnerMessage.SetActive(true); // Muestra el mensaje

        // Cambia el texto según el jugador derrotado
        TextMeshProUGUI winnerText = winnerMessage.GetComponentInChildren<TextMeshProUGUI>();
        if (gameObject.name.Contains("Left")) // Si el jugador izquierdo muere
        {
            winnerText.text = "Ganador: Jugador 2";
        }
        else if (gameObject.name.Contains("Right")) // Si el jugador derecho muere
        {
            winnerText.text = "Ganador: Jugador 1";
        }
    }
    else{
        Debug.LogError("winnerMessage is not assigned");
    }
} */

    public float GetCurrentHealth()
{
    return currentHealth;
}
}
