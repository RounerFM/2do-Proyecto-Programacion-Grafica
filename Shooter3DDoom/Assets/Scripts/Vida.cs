using UnityEngine;
using TMPro;

public class Vida : MonoBehaviour
{
    public int vidaMax = 3;
    public bool esJugador = false;
    public TMP_Text vidaTexto;    // opcional: HUD de vida del jugador
    public AudioClip sonidoDano;  // opcional: sonido al recibir daño

    private int vidaActual;
    private AudioSource fuente;

    void Start()
    {
        vidaActual = vidaMax;
        fuente = GetComponent<AudioSource>();
        ActualizarHUD();
    }

    public void RecibirDano(int cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;

        // Feedback de daño (flash rojo) solo para el jugador
        if (esJugador && DanoFlash.Instancia != null)
            DanoFlash.Instancia.Flash();

        if (sonidoDano != null && fuente != null)
            fuente.PlayOneShot(sonidoDano);

        ActualizarHUD();

        if (vidaActual <= 0) Morir();
    }

    public void Curar(int cantidad)
    {
        vidaActual = Mathf.Min(vidaActual + cantidad, vidaMax);
        ActualizarHUD();
    }

    void Morir()
    {
        if (esJugador)
        {
            // Menú de Game Over (si existe); si no, reinicia directo
            if (GameOverManager.Instancia != null)
                GameOverManager.Instancia.MostrarGameOver();
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        else
        {
            if (GameManager.Instancia != null)
                GameManager.Instancia.EnemigoMuerto();
            Destroy(gameObject);
        }
    }

    void ActualizarHUD()
    {
        if (esJugador && vidaTexto != null)
            vidaTexto.text = "Vida: " + vidaActual + " / " + vidaMax;
    }

    public int VidaActual()
    {
        return vidaActual;
    }
}
