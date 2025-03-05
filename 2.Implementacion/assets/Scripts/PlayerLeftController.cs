using System.Collections;
using UnityEngine;

public class PlayerLeftController : MonoBehaviour
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
    public GameObject fireballLeftPrefab; // Prefab de la bola del jugador izquierdo
    private SpriteRenderer spriteRenderer;

    public AudioClip hitAirSound; // Sonido para golpe al aire
    public AudioClip hitMatchSound; // Sonido para golpe al rival

    public float jumpForceY = 8.8f; // Fuerza vertical del salto
    public float jumpForceX = 0f;  // Fuerza horizontal del salto (hacia la derecha)

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
        if (Input.GetKeyDown(KeyCode.X) && canShootFireball)
        {
            LaunchFireball(fireballLeftPrefab); // Lanzar bola de fuego
            StartCoroutine(DisableShootingTemporarily()); // Desactivar la tecla temporalmente
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Para saber si está en el suelo (control para evitar saltos en el aire)

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            // Aplica la fuerza del salto
            Vector2 jumpForce = new Vector2(jumpForceX, jumpForceY); // Fuerza hacia arriba y hacia la derecha
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0); // Resetea la velocidad actual
            GetComponent<Rigidbody2D>().AddForce(jumpForce, ForceMode2D.Impulse); // Aplica la fuerza
        }

        // Movimiento horizontal solo con A y D
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f; // Movimiento hacia la izquierda
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f; // Movimiento hacia la derecha
        }

        Vector3 movement = new Vector3(moveInput, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Cambiar sprite al pulsar Q (patada) y llamar a la función Attack
        if (Input.GetKeyDown(KeyCode.Q))
        {
            spriteRenderer.sprite = kickSprite;
            Attack(); // Llamamos a la función de ataque
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            spriteRenderer.sprite = punchSprite;
            Attack(); // Llamamos a la función de ataque
        }

        // Volver al sprite de reposo si no se está atacando
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            spriteRenderer.sprite = idleSprite;
        }

        // Lógica de defensa
        if (Input.GetKeyDown(KeyCode.S)) // Activar defensa al pulsar S
        {
            isDefending = true;
            spriteRenderer.sprite = defenseSprite; // Cambiar al sprite de defensa
        }
        if (Input.GetKeyUp(KeyCode.S)) // Desactivar defensa al soltar S
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
        launchPosition.x += 3f; // Separación de 3 unidades desde el jugador hasta donde sale la bola
        launchPosition.y += 0.5f; // Ajuste para que la bola salga a la altura de las manos

        // Instancia la bola en la posición calculada
        GameObject fireball = Instantiate(fireballPrefab, launchPosition, Quaternion.identity);

        // Obtén el componente FireballController de la bola
        FireballController fireballController = fireball.GetComponent<FireballController>();

        // Define la dirección de la bola (hacia la derecha para el jugador izquierdo)
        fireballController.direction = Vector2.right;
    }

    IEnumerator DisableShootingTemporarily()
    {
        canShootFireball = false; // Desactiva la posibilidad de lanzar bolas de fuego
        yield return new WaitForSeconds(0.5f); // Espera 0.5 segundos
        canShootFireball = true; // Reactiva la posibilidad de lanzar bolas de fuego
    }
}