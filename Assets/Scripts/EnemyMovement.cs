using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip muerteAudio;

    public float velocidadMovimiento = 2f;
    private Rigidbody2D rb;

    public int daño = 10;
    public int vida = 1000;

    private bool movimientoDerecha = true;

    public float rangoVision = 5f;
    public LayerMask capaJugador;
    public LayerMask CapaObstaculos;
    private Transform jugador;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (jugador != null && PuedeVerJugador())
        {
            Vector2 direccion = (jugador.position - transform.position);
            direccion.Normalize(); // Normalizar para mantener la dirección pero no alterar demasiado los valores
            rb.velocity = direccion * velocidadMovimiento;
            CambiarOrientacion(direccion.x);
        }
        else
        {
            rb.velocity = new Vector2((movimientoDerecha ? 1 : -1) * velocidadMovimiento, rb.velocity.y);
        }
    }

    private bool PuedeVerJugador()
    {
        float distanciaJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaJugador <= rangoVision)
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;
            // Opcional: Chequear si hay obstáculos entre el enemigo y el jugador
            RaycastHit2D hit = Physics2D.Raycast(transform.position, jugador.position - transform.position, rangoVision, CapaObstaculos);
            if (hit.collider == null) // Si no hay obstáculos, el enemigo puede "ver" al jugador
            {
                return true;
            }
        }
        return false;
    }

    private void CambiarOrientacion(float direccionX)
    {
        if (direccionX > 0 && !movimientoDerecha || direccionX < 0 && movimientoDerecha)
        {
            movimientoDerecha = !movimientoDerecha; // Cambiar la dirección
            Vector3 escala = transform.localScale;
            escala.x *= -1; // Invertir el eje X
            transform.localScale = escala;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detecta colisiones con objetos etiquetados como "Ground" (las paredes)
        if (collision.gameObject.CompareTag("Ground"))
        {
            CambiarDireccion();
        }


        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement jugador = collision.gameObject.GetComponent<PlayerMovement>();
            if (jugador != null)
            {
                jugador.RecibirDaño(daño);
            }
        }
    }

    private void CambiarDireccion()
    {
        movimientoDerecha = !movimientoDerecha; // Cambiar la dirección
        Vector3 escala = transform.localScale; // Cambiar la orientación del sprite
        escala.x *= -1; // Invertir el eje X
        transform.localScale = escala;
    }


    public void RecibirDaño(int daño, Vector2 direccionEmpuje, float fuerzaEmpuje)
    {
        vida -= daño; ;

        Debug.Log($"Enemigo recibió {daño} de daño. Vida restante: {vida}");

        if (audioSource != null)
        {
            audioSource.Play();
        }

        rb.AddForce(direccionEmpuje * fuerzaEmpuje, ForceMode2D.Impulse);

        if (vida <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        if (audioSource != null && muerteAudio != null)
        {
            audioSource.PlayOneShot(muerteAudio);
        }

        Debug.Log("El enemigo ha muerto.");

        GameObject.FindObjectOfType<GameManager>()?.EnemigoMuerto(gameObject);

        Destroy(gameObject, 0.8f);
    }
}
