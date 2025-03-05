using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Ajusta el tamaño del Sprite a la pantalla
        float worldScreenWidth = mainCamera.orthographicSize * 2f * (Screen.width / Screen.height);
        float worldScreenHeight = mainCamera.orthographicSize * 2f;

        // Calcula el tamaño del Sprite
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Calcula el factor de escala
        float scaleX = worldScreenWidth / spriteSize.x;
        float scaleY = worldScreenHeight / spriteSize.y;

        // Aplica el escala
        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}

