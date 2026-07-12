using UnityEngine;
using UnityEngine.UI;

// Muestra la vida del jugador con imágenes de corazón.
// Poner en el Canvas (o en un objeto vacío del Canvas).
public class VidaCorazones : MonoBehaviour
{
    public Vida vidaJugador;        // el componente Vida del Jugador
    public Image[] corazones;       // arrastra aquí los corazones EN ORDEN
    public Sprite corazonLleno;     // sprite rojo
    public Sprite corazonVacio;     // sprite gris (opcional)

    void Update()
    {
        if (vidaJugador == null) return;

        int vida = vidaJugador.VidaActual();

        for (int i = 0; i < corazones.Length; i++)
        {
            if (corazones[i] == null) continue;

            bool lleno = i < vida;

            if (corazonVacio != null)
            {
                // Modo lleno/vacío: cambia el sprite del corazón
                corazones[i].sprite = lleno ? corazonLleno : corazonVacio;
                corazones[i].enabled = true;
            }
            else
            {
                // Modo simple: el corazón perdido desaparece
                corazones[i].enabled = lleno;
            }
        }
    }
}
