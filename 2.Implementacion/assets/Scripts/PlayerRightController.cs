using System.Collections;
using UnityEngine;

public class PlayerRightController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public Sprite idleSprite;    // Sprite cuando está parado
    public Sprite kickSprite;    // Sprite de patada
    public Sprite punchSprite;   // Sprite de puñetazo
    public Sprite defenseSprite; // Sprite de defensa
    public bool isDefending = false; // Variable para saber si el jugador está defendiendo
    public float attackRange = 1.5f; // Rango de golpe
    public LayerMask enemyLayer; // Para detectar al otro jugador
    public float damage = 20f; // Daño por golpe
    public GameObject fireballRightPrefab; // Prefab de la bola del jugador derecho
    private SpriteRenderer spriteRenderer;

    public AudioClip hitAirSound; // Sonido para golpe al aire
    public AudioClip hitMatchSound; // Sonido para golpe al rival

    public float jumpForceY = 8.8f; // Fuerza vertical del salto
    public float jumpForceX = 0f;  // Fuerza horizontal del salto (hacia la izquierda)

    public Transform groundCheck;  // Punto de verificación del suelo
    public float groundCheckRadius = 0.2f; // Radio de detección del suelo
    public LayerMask groundLayer;  // Capa del suelo
    private bool isGrounded = false; // Indica si el jugador está en el suelo

    private bool canShootFireball = true; // Indica si el jugador puede lanzar una bola de fuego

    void Start()
    {
        // Obtén el componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2) && canShootFireball)
        {
            LaunchFireball(fireballRightPrefab); // Lanzar bola de fuego
            StartCoroutine(DisableShootingTemporarily()); // Desactivar la tecla temporalmente
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Para saber si está en el suelo (control para evitar saltos en el aire)

        if (Input.GetKeyDown(KeyCode.Keypad8) && isGrounded)
        {
            // Aplica la fuerza del salto
            Vector2 jumpForce = new Vector2(-jumpForceX, jumpForceY); // Fuerza hacia arriba y hacia la izquierda
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0); // Resetea la velocidad actual
            GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse); // Aplica la fuerza
        }

        // Movimiento horizontal con las teclas del teclado numérico 4 (izquierda) y 6 (derecha)
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.Keypad4)) moveInput = -1f;
        if (Input.GetKey(KeyCode.Keypad6)) moveInput = 1f;

        Vector3 movement = new Vector3(moveInput, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Cambiar sprite al pulsar 7 (patada) del teclado numérico
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            spriteRenderer.sprite = kickSprite;
            Attack(); // Llamamos a la función de ataque
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            spriteRenderer.sprite = punchSprite;
            Attack(); // Llamamos a la función de ataque
        }

        // Volver al sprite de reposo si no se está atacando
        if (Input.GetKeyUp(KeyCode.Keypad7) || Input.GetKeyUp(KeyCode.Keypad9))
        {
            spriteRenderer.sprite = idleSprite;
        }

        // Lógica de defensa
        if (Input.GetKeyDown(KeyCode.Keypad5)) // Activar defensa al pulsar 5
        {
            isDefending = true;
            spriteRenderer.sprite = defenseSprite; // Cambiar al sprite de defensa
        }
        if (Input.GetKeyUp(KeyCode.Keypad5)) // Desactivar defensa al soltar 5
        {
            isDefending = false;
            spriteRenderer.sprite = idleSprite; // Volver al sprite normal
        }
    }

    void Attack()
    {
        Debug.Log(gameObject.name + " intentó atacar");

        // Detectar colisiones en un círculo alrededor del jugador
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, attackRange, enemyLayer);

        if (enemy != null)
        {
            Debug.Log(gameObject.name + " golpeó a " + enemy.name);

            // Reproducir sonido de golpe al rival
            if (hitMatchSound != null)
            {
                AudioSource.PlayClipAtPoint(hitMatchSound, transform.position);
            }

            // Obtener el componente PlayerHealth del enemigo
            PlayerHealth enemyHealth = enemy.GetComponent<PlayerHealth>();
            if (enemyHealth != null)
            {
                // Aplicar daño al enemigo
                enemyHealth.TakeDamage(damage);
            }
        }
        else
        {
            // Reproducir sonido de golpe al aire
            if (hitAirSound != null)
            {
                AudioSource.PlayClipAtPoint(hitAirSound, transform.position);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void LaunchFireball(GameObject fireballPrefab)
    {
        // Calcula la posición de lanzamiento (ligeramente desplazada del jugador)
        Vector2 launchPosition = transform.position;
        launchPosition.x -= 3f; // Ajusta este valor según el tamaño del collider del jugador
        launchPosition.y += 0.5f; // Ajuste para que la bola salga a la altura de las manos

        // Instancia la bola en la posición calculada
        GameObject fireball = Instantiate(fireballPrefab, launchPosition, Quaternion.identity);

        // Obtén el componente FireballController de la bola
        FireballController fireballController = fireball.GetComponent<FireballController>();

        // Define la dirección de la bola (hacia la izquierda para el jugador derecho)
        fireballController.direction = Vector2.left;
    }

    IEnumerator DisableShootingTemporarily()
    {
        canShootFireball = false; // Desactiva la posibilidad de lanzar bolas de fuego
        yield return new WaitForSeconds(0.5f); // Espera 0.5 segundos
        canShootFireball = true; // Reactiva la posibilidad de lanzar bolas de fuego
    }
}