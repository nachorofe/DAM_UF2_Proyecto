using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Método para iniciar el juego
    public void StartGame()
    {
        SceneManager.LoadScene("Game"); // Asegúrate de que la escena del juego se llame "Game"
    }

    // Método para salir del juego
    public void ExitGame()
    {
        Application.Quit(); // Cierra la aplicación
        Debug.Log("El juego se ha cerrado"); // Solo visible en el editor
    }
}

