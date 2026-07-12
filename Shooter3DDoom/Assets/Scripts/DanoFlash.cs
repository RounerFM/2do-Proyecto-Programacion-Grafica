using UnityEngine;
using UnityEngine.UI;

// Poner en el Canvas. Necesita una Image roja a pantalla completa.
// Cuando el jugador recibe daño, la pantalla parpadea en rojo.
public class DanoFlash : MonoBehaviour
{
    public static DanoFlash Instancia;

    public Image imagenRojo;    // Image roja full-screen
    public float duracion = 0.4f;
    public float alphaMax = 0.5f;

    private float t = 0f;

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
        if (imagenRojo != null) PonerAlpha(0f);
    }

    // Vida.cs llama a esto cuando el jugador recibe daño
    public void Flash()
    {
        t = duracion;
    }

    void Update()
    {
        if (imagenRojo == null) return;

        if (t > 0f)
        {
            t -= Time.deltaTime;
            PonerAlpha(Mathf.Clamp01(t / duracion) * alphaMax);
        }
    }

    void PonerAlpha(float a)
    {
        Color c = imagenRojo.color;
        c.a = a;
        imagenRojo.color = c;
    }
}
