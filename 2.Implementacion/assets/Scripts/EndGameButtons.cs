using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameButtons : MonoBehaviour
{
    public void OnNewGameButtonClicked()
    {
        // Reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMenuButtonClicked()
    {
        // Carga la escena del menú
        SceneManager.LoadScene("Menu");
    }
}