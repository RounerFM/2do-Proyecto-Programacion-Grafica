using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instancia;
    public GameObject panelGameOver;  // panel con el botón "Reintentar"

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
        if (panelGameOver != null) panelGameOver.SetActive(false);
    }

    // Vida.cs llama a esto cuando muere el jugador
    public void MostrarGameOver()
    {
        if (panelGameOver != null) panelGameOver.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;   // congela el juego
    }

    // Enlazar al OnClick del botón "Reintentar"
    public void Reintentar()
    {
        Time.timeScale = 1f;   // reanuda el tiempo antes de recargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
