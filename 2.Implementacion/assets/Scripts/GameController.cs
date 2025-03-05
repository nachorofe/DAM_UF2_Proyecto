using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private bool isPaused = false;
    public GameObject pauseIcon; // Referencia al icono de pausa

    void Update()
    {
        // Si el jugador presiona la tecla Escape, vuelve al menú
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }

    // Si el jugador presiona la tecla P, alterna la pausa
    if (Input.GetKeyDown(KeyCode.P))
    {
        TogglePause();
    }

    }

void TogglePause()
{
    isPaused = !isPaused; // Alterna el estado de pausa
    Time.timeScale = isPaused ? 0 : 1; // Pausa o reanuda el tiempo del juego

    // Activa o desactiva el icono de pausa
    if (pauseIcon != null)
    {
        pauseIcon.SetActive(isPaused);
    }

    // Desactiva o reactiva los scripts de movimiento y ataque
    PlayerLeftController leftController = GameObject.Find("PlayerLeft").GetComponent<PlayerLeftController>();
    PlayerRightController rightController = GameObject.Find("PlayerRight").GetComponent<PlayerRightController>();

    if (leftController != null) leftController.enabled = !isPaused;
    if (rightController != null) rightController.enabled = !isPaused;
}

}
