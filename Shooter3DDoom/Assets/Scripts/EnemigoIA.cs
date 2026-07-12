using UnityEngine;
using UnityEngine.AI;

public enum TipoEnemigo { Normal, Rapido, Ralentizador }

[RequireComponent(typeof(NavMeshAgent))]
public class EnemigoIA : MonoBehaviour
{
    [Header("Tipo de enemigo")]
    public TipoEnemigo tipo = TipoEnemigo.Normal;
    public float velocidadMovimiento = 3.5f;  // velocidad al perseguir

    [Header("Ralentizador (solo si tipo = Ralentizador)")]
    public float factorRalentizacion = 0.1f;  // 0.1 = el jugador va al 10% (90% más lento)
    public float duracionRalentizacion = 3f;   // segundos que dura el efecto

    [Header("Detección y disparo")]
    public float rangoDeteccion = 20f; // a qué distancia empieza a perseguir
    public float rangoDisparo = 12f;   // a qué distancia dispara
    public int dano = 1;
    public float cadencia = 1.5f;      // segundos entre disparos
    public AudioClip sonidoDisparo;

    private NavMeshAgent agente;
    private Transform jugador;
    private Vida jugadorVida;
    private PrimeraPersona jugadorMovimiento;
    private AudioSource fuente;
    private float proximoDisparo = 0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        fuente = GetComponent<AudioSource>();

        agente.speed = velocidadMovimiento;  // el rápido tendrá un valor mayor

        GameObject j = GameObject.FindGameObjectWithTag("Player");
        if (j != null)
        {
            jugador = j.transform;
            jugadorVida = j.GetComponent<Vida>();
            jugadorMovimiento = j.GetComponent<PrimeraPersona>();
        }

        if (GameManager.Instancia != null)
            GameManager.Instancia.RegistrarEnemigo();
    }

    void Update()
    {
        if (jugador == null) return;

        float dist = Vector3.Distance(transform.position, jugador.position);

        // Perseguir
        if (dist <= rangoDeteccion && agente.isOnNavMesh)
            agente.SetDestination(jugador.position);

        // Disparar si está cerca y tiene línea de visión
        if (dist <= rangoDisparo && Time.time >= proximoDisparo && TieneLinea())
        {
            proximoDisparo = Time.time + cadencia;
            DispararAlJugador();
        }
    }

    bool TieneLinea()
    {
        // Apuntamos al centro del cuerpo (no a la cabeza) para no pasar por encima
        Vector3 origen = transform.position + Vector3.up * 0.5f;
        Vector3 objetivo = jugador.position + Vector3.up * 0.5f;
        Vector3 dir = objetivo - origen;
        float d = dir.magnitude;
        dir /= d;

        // Arrancar el rayo un poco fuera del propio collider del enemigo
        origen += dir * 0.6f;

        if (Physics.Raycast(origen, dir, out RaycastHit hit, d))
            return hit.collider.CompareTag("Player");
        return false;
    }

    void DispararAlJugador()
    {
        if (sonidoDisparo != null && fuente != null) fuente.PlayOneShot(sonidoDisparo);
        if (jugadorVida != null) jugadorVida.RecibirDano(dano);

        // El ralentizador, además del daño, deja al jugador caminando lento
        if (tipo == TipoEnemigo.Ralentizador && jugadorMovimiento != null)
            jugadorMovimiento.AplicarRalentizacion(factorRalentizacion, duracionRalentizacion);
    }
}
