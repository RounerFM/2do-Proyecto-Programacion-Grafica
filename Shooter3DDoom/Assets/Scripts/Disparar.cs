using UnityEngine;
using TMPro;
using System.Collections;

public class Disparar : MonoBehaviour
{
    public Camera camara;
    public int dano = 2;
    public float alcance = 100f;
    public float cadencia = 0.5f;
    public AudioClip sonidoDisparo;
    public GameObject muzzle;

    [Header("Munición")]
    public int municionMax = 12;       // balas por cargador
    public float tiempoRecarga = 1.5f; // segundos que tarda la R
    public TMP_Text municionTexto;     // el texto del HUD

    private AudioSource fuente;
    private float proximo = 0f;
    private int municionActual;
    private bool recargando = false;

    void Start()
    {
        fuente = GetComponent<AudioSource>();
        if (muzzle != null) muzzle.SetActive(false);

        municionActual = municionMax;
        ActualizarHUD();
    }

    void Update()
    {
        // Recargar con R (si no está llena y no está recargando ya)
        if (Input.GetKeyDown(KeyCode.R) && !recargando && municionActual < municionMax)
        {
            StartCoroutine(Recargar());
            return;
        }

        // Disparar: solo si hay balas y no está recargando
        if (Input.GetMouseButtonDown(0) && Time.time >= proximo && !recargando && municionActual > 0)
        {
            proximo = Time.time + cadencia;
            municionActual--;
            ActualizarHUD();
            Disparo();
        }
    }

    void Disparo()
    {
        if (sonidoDisparo != null) fuente.PlayOneShot(sonidoDisparo);
        if (muzzle != null)
        {
            muzzle.SetActive(true);
            Invoke("ApagarMuzzle", 0.05f);
        }

        Ray ray = camara.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, alcance))
        {
            Vida v = hit.collider.GetComponentInParent<Vida>();
            if (v != null) v.RecibirDano(dano);
        }
    }

    IEnumerator Recargar()
    {
        recargando = true;
        ActualizarHUD();
        yield return new WaitForSeconds(tiempoRecarga);
        municionActual = municionMax;
        recargando = false;
        ActualizarHUD();
    }

    void ActualizarHUD()
    {
        if (municionTexto == null) return;
        municionTexto.text = recargando
            ? "RECARGANDO..."
            : "Munición: " + municionActual + " / " + municionMax;
    }

    void ApagarMuzzle()   // antes decía "ApaagarMuzzle" (bug corregido)
    {
        if (muzzle != null) muzzle.SetActive(false);
    }
}
