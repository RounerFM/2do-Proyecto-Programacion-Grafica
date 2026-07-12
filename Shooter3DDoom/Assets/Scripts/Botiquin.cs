using UnityEngine;

// Poner en el objeto del botiquín. Necesita un Collider "Is Trigger".
// Al tocarlo, el jugador recupera vida (con tope en el máximo) y el
// botiquín desaparece con un sonido.
public class Botiquin : MonoBehaviour
{
    public int curacion = 1;
    public AudioClip sonidoRecoger;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Vida v = other.GetComponent<Vida>();
        if (v != null) v.Curar(curacion);

        if (sonidoRecoger != null)
            AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);

        Destroy(gameObject);
    }
}
