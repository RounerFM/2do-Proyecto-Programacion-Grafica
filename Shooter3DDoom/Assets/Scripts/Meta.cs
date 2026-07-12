using UnityEngine;

// Poner en un objeto con un Collider marcado como "Is Trigger".
// Cuando el jugador entra, avisa al GameManager (una de las dos
// condiciones de victoria).
public class Meta : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instancia != null)
            GameManager.Instancia.JugadorLlegoMeta();
    }
}
