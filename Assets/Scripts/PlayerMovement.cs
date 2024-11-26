using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMovimiento = 5f;
    public float FuerzaSalto = 5f;

    [Header("Vida del jugador")]
    public int vidaMaxima = 100;
    private int vidaActual;

    [Header("Barra de vida")]
    public Image barraDeVida;

    [Header("Retroceso")]
    public float fuerzaRetroceso = 10f;


    [Header("Ataque")]
    public float rangoAtaque = 2f;
    public int dañoAtaque = 20;
    public LayerMask enemigoLayer;

    private Rigidbody2D rb;
    private bool EnContactoConElPiso;

    private int contadorSaltos;
    public int maxSaltos = 2;

    private bool estaSiendoEmpujado = false;

    [Header("Sprite Arma")]
    public GameObject arma;
    public float tiempoArmaVisible = 0.3f;

    [Header("Sprite de Muerte")]
    public GameObject spriteFinaly;

    public AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        vidaActual = vidaMaxima;
        contadorSaltos = 0;

        if (arma != null)
        {
            arma.SetActive(false);
        }

        if (spriteFinaly != null)
        {
            spriteFinaly.SetActive(false); // Desactiva el sprite al inicio
        }


        ActualizarBarraDeVida();
    }

    void Update()
    {
            // Movimiento horizontal
            float movimiento = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(movimiento * velocidadMovimiento, rb.velocity.y);

            if (movimiento < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (movimiento > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Salto
            if (Input.GetButtonDown("Jump") && contadorSaltos < maxSaltos)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * FuerzaSalto, ForceMode2D.Impulse);
                contadorSaltos++;
            }

        if (Input.GetMouseButtonDown(1))
        {
            Atacar();
        }
    }

    private void ActualizarBarraDeVida() 
    {
        if (barraDeVida != null) 
        {
            float procentajeVida = (float)vidaActual / vidaMaxima;
            barraDeVida.fillAmount = procentajeVida; ;
        }
    }


    private void Atacar()
    {
        Debug.Log("Intentando atacar...");

        if (arma != null)
        {
            arma.SetActive(true);
            StartCoroutine(DesactivarArmaDespuesDeTiempo());
        }
    }


    private IEnumerator DesactivarArmaDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoArmaVisible); // Esperar el tiempo configurado
        if (arma != null)
        {
            arma.SetActive(false); // Desactivar el arma
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            EnContactoConElPiso = true;
            contadorSaltos = 0;
        }

        if (collision.gameObject.CompareTag("Enemy") && !estaSiendoEmpujado)
        {
            Debug.Log($"Colisión detectada con: {collision.gameObject.name}");

            // Obtener el daño del enemigo
            int daño = collision.gameObject.GetComponent<EnemyMovement>()?.daño ?? 10;
            RecibirDaño(daño);

        }

    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            EnContactoConElPiso = false;
        }
    }


    public void RecibirDaño(int daño)
    {
        vidaActual -= daño;

        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarBarraDeVida();

        // Actualizar barra de vida
        if (barraDeVida != null)
        {
            float porcentajeVida = (float)vidaActual / vidaMaxima;
        }

        Debug.Log($"Jugador recibió {daño} de daño. Vida restante: {vidaActual}");

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");

        if (spriteFinaly != null)
        {

            if (audioSource != null && audioSource.clip != null)
            {
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            }

            spriteFinaly.SetActive(true);
            Destroy(gameObject);

            GameObject.FindObjectOfType<GameManager>()?.SetJugadorMuerto();
        }

        gameObject.SetActive(false);

    }
    

}