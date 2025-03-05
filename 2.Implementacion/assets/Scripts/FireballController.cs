using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float speed = 10f; // Velocidad de la bola
    public int damage = 5; // Daño que causa la bola
    public Vector2 direction; // Dirección en la que se mueve la bola
    public AudioClip hitMatchSound; // Sonido para golpe al rival

    void Update()
    {
        // Mueve la bola en la dirección especificada
        transform.Translate(direction * speed * Time.deltaTime);
    }

void OnTriggerEnter2D(Collider2D collision)
{
    // Verifica si la bola choca con un jugador
    PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
    if (playerHealth != null)
    {
        // Reproducir sonido de golpe al rival
        if (hitMatchSound != null)
        {
            AudioSource.PlayClipAtPoint(hitMatchSound, transform.position);
        }

        // Verifica si el jugador está defendiendo
        PlayerLeftController leftController = collision.GetComponent<PlayerLeftController>();
        PlayerRightController rightController = collision.GetComponent<PlayerRightController>();

        bool isDefending = false;

        if (leftController != null)
        {
            isDefending = leftController.isDefending;
        }
        else if (rightController != null)
        {
            isDefending = rightController.isDefending;
        }

        // Aplica el daño según si el jugador está defendiendo o no
        if (!isDefending)
        {
            playerHealth.TakeDamage(damage); // Daño normal
        }
        else
        {
            playerHealth.TakeDamage(1); // Daño reducido (1) si está defendiendo
        }

        // Destruye la bola después del impacto
        Destroy(gameObject);
    }
    else
    {
        // Destruye la bola si choca con algo que no es un jugador
        Destroy(gameObject);
    }
}
}