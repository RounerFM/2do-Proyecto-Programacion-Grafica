using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    [Header("HUD y victoria")]
    public TMP_Text contadorTexto;    // "Enemigos: X"
    public GameObject panelVictoria;  // panel que aparece al ganar
    public AudioClip sonidoVictoria;

    private int enemigosVivos = 0;
    private bool jugadorEnMeta = false;
    private bool nivelTerminado = false;

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
        if (panelVictoria != null) panelVictoria.SetActive(false);
        ActualizarContador();
    }

    // Cada enemigo llama a esto en su Start
    public void RegistrarEnemigo()
    {
        enemigosVivos++;
        ActualizarContador();
    }

    // Vida.cs llama a esto cuando muere un enemigo
    public void EnemigoMuerto()
    {
        enemigosVivos--;
        if (enemigosVivos < 0) enemigosVivos = 0;
        ActualizarContador();
        ComprobarVictoria();
    }

    // La zona Meta llama a esto cuando el jugador la pisa
    public void JugadorLlegoMeta()
    {
        jugadorEnMeta = true;
        ComprobarVictoria();
    }

    void ComprobarVictoria()
    {
        if (nivelTerminado) return;

        // Condición DOBLE: llegar a la meta Y matar a todos
        if (enemigosVivos == 0 && jugadorEnMeta)
        {
            nivelTerminado = true;

            if (sonidoVictoria != null && Camera.main != null)
                AudioSource.PlayClipAtPoint(sonidoVictoria, Camera.main.transform.position);

            if (panelVictoria != null) panelVictoria.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }

    void ActualizarContador()
    {
        if (contadorTexto != null)
            contadorTexto.text = "Enemigos: " + enemigosVivos;
    }
}
