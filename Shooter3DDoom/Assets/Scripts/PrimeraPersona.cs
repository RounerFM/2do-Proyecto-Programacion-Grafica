using UnityEngine;

public class PrimeraPersona : MonoBehaviour
{
    
    public float velocidad = 5f;
    public float sensibilidad = 2f;
    public float gravedad = -9.81f;
    public Transform camara;

    private CharacterController cc;
    private float pitch = 0f;
    private Vector3 velY;

    private float velocidadActual;
    private float finRalentizacion = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        velocidadActual = velocidad;
    }

    // La llama el enemigo ralentizador cuando te alcanza
    public void AplicarRalentizacion(float factor, float duracion)
    {
        velocidadActual = velocidad * factor;
        finRalentizacion = Time.time + duracion;
    }

    void Update()
    {
        // Si terminó el efecto, recupera la velocidad normal
        if (Time.time >= finRalentizacion) velocidadActual = velocidad;

        //Mirar con el raton
        float mx = Input.GetAxis("Mouse X") * sensibilidad;
        float my = Input.GetAxis("Mouse Y") * sensibilidad;
        transform.Rotate(0, mx, 0); // girar el cuerpo
        pitch = Mathf.Clamp(pitch - my, -80f, 80f); // mirar arriba y abajo
        camara.localEulerAngles = new Vector3(pitch, 0, 0);

        // Caminar (WASD o Flechas)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 mov = (transform.right * h + transform.forward * v).normalized * velocidadActual;

        // Gravedad simple
        if (cc.isGrounded && velY.y < 0) velY.y = -2f;
        velY.y += gravedad * Time.deltaTime;

        cc.Move((mov + velY) * Time.deltaTime);

    }
}
