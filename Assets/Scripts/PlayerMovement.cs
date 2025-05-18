using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;
    public LayerMask suelo;

    public VidaJugadorUI uiVida;

    public int vidaMaxima = 5;
    private int vidaActual;

    private bool estaMuerto = false;

    private Rigidbody2D rb;
    private bool enSuelo;

    public Transform detectorSuelo;
    public float radioDeteccion = 0.4f;

    public Vector2 ultimoCheckpoint;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float alturaInicioCaida;
    public float alturaMinimaParaCaer = 0.1f;

    [Header("Dash")]
    public float fuerzaDash = 40f;
    public float tiempoDash = 0.683f;
    private bool haciendoDash = false;
    private bool puedeHacerDash = true;
    private float gravedadOriginal;

    private bool animacionDashActiva = false;
    private bool puedeCorrer = true;
    private float tiempoBloqueoCorrer = 0.3f;

    [Header("Slide")]
    public KeyCode teclaSlide = KeyCode.S;
    public float tiempoSlide = 0.5f;
    private bool haciendoSlide = false;

    [Header("Rolling")]
    public KeyCode teclaRolling = KeyCode.C;
    public float tiempoRolling = 0.5f;
    private bool haciendoRolling = false;

    [Header("AFK / Sentarse")]
    public float tiempoParaAFK = 15f;
    private float tiempoInactivo = 0f;
    private bool afk = false;
    private bool sentado = false;
    private bool levantandose = false;

    [Header("Ataques")]
    public float duracionAtaque = 0.7f;
    private bool atacando = false;


    [Header("Hitbox de Ataque")]
    [SerializeField] private GameObject hitboxAtaque;


    private BoxCollider2D boxCollider;
    private Vector2 colliderSizeOriginal;
    private Vector2 colliderOffsetOriginal;

    [SerializeField] private Vector2 colliderSizeSlide = new Vector2(1f, 0.4f);
    [SerializeField] private Vector2 colliderOffsetSlide = new Vector2(0f, 0.1f);

    private bool puedeSaltoExtra = false;


    void Start()
    {
        vidaActual = vidaMaxima;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ultimoCheckpoint = transform.position;
        animator.Play("idle1");
        gravedadOriginal = rb.gravityScale;
        boxCollider = GetComponent<BoxCollider2D>();
        colliderSizeOriginal = boxCollider.size;
        colliderOffsetOriginal = boxCollider.offset;
    }

    void Update()
    {

        if (estaMuerto) return;

        bool estabaEnSuelo = enSuelo;
        enSuelo = Physics2D.OverlapCircle(detectorSuelo.position, radioDeteccion, suelo);
        animator.SetBool("enSuelo", enSuelo);

        float movimiento = Input.GetAxisRaw("Horizontal");

        if (levantandose)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("falling", false);
            return;
        }

        if (estabaEnSuelo && !enSuelo)
        {
            alturaInicioCaida = transform.position.y;
        }

        // DASH
        if (!haciendoDash && !animacionDashActiva && Input.GetKeyDown(KeyCode.LeftShift) && puedeHacerDash)
        {
            StartCoroutine(Dash());
        }

        // ROLLING
        if (Input.GetKeyDown(teclaRolling) && enSuelo && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f && !haciendoRolling)
        {
            StartCoroutine(HacerRolling());
        }

        // SLIDE
        if (Input.GetKeyDown(teclaSlide) && enSuelo && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f && !haciendoSlide)
        {
            StartCoroutine(RealizarSlide());
        }

        // ATAQUE
        // ATAQUES INDIVIDUALES CON TECLAS 1, 2 Y 3
        if (!haciendoDash && !haciendoRolling && !haciendoSlide && !atacando)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(RealizarAtaque(1));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(RealizarAtaque(2));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(RealizarAtaque(3));
            }
        }



        if (haciendoDash || animacionDashActiva)
        {
            animator.SetBool("Dash", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("falling", false);
            return;
        }

        if (haciendoRolling)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("falling", false);
            return;
        }

        // SALTO
       if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enSuelo)
            {
                rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
                animator.SetTrigger(Mathf.Abs(movimiento) > 0.01f ? "saltomovimiento" : "salto");
                animator.SetBool("isRunning", false);
            }
            else if (puedeSaltoExtra)
            {
                rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
                animator.SetTrigger(Mathf.Abs(movimiento) > 0.01f ? "saltomovimiento" : "salto");
                puedeSaltoExtra = false; // consumir el salto extra
            }
        }


        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (enSuelo)
        {
            puedeHacerDash = true;
            animator.SetBool("falling", false);
            animator.SetBool("Dash", false);

            if (haciendoSlide)
            {
                animator.SetBool("isRunning", false);
            }
            else if (puedeCorrer && Mathf.Abs(movimiento) > 0.01f)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
        else
        {
            float alturaCaida = alturaInicioCaida - transform.position.y;
            animator.SetBool("falling", rb.velocity.y < 0 && alturaCaida >= alturaMinimaParaCaer);
            if (rb.velocity.y > 0)
                animator.SetBool("falling", false);

            animator.SetBool("isRunning", false);
        }

        // -------- DETECCIÓN DE INACTIVIDAD --------
        bool hayActividad = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f ||
                            Input.GetKeyDown(KeyCode.Space) ||
                            Input.GetKeyDown(KeyCode.LeftShift) ||
                            Input.GetKeyDown(teclaRolling) ||
                            Input.GetKeyDown(teclaSlide);

        if (!hayActividad && enSuelo && !afk && !sentado && !atacando)
        {
            tiempoInactivo += Time.deltaTime;

            if (tiempoInactivo >= tiempoParaAFK)
            {
                afk = true;
                sentado = true;
                animator.SetBool("afk", true);
                tiempoInactivo = 0f;
            }
        }
        else if (hayActividad)
        {
            if (afk || sentado)
            {
                afk = false;
                animator.SetBool("afk", false);

                if (!levantandose)
                {
                    levantandose = true;
                    StartCoroutine(Levantarse());
                }
            }

            tiempoInactivo = 0f;
        }
    }

    void FixedUpdate()
    {
        if (levantandose || haciendoDash || atacando || estaMuerto) return;


        float movimiento = Input.GetAxis("Horizontal");

        if (movimiento != 0)
            spriteRenderer.flipX = movimiento < 0;

        float velocidadActual = enSuelo ? velocidad : Mathf.Max(velocidad, velocidad * 1.2f);
        rb.velocity = new Vector2(movimiento * velocidadActual * Time.deltaTime * 60, rb.velocity.y);

        rb.gravityScale = !enSuelo && rb.velocity.y < 0 ? 3f : 2f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            ActualizarCheckpoint(collision.transform.position);
        }
        else if (collision.CompareTag("ZonaMuerte"))
        {
            transform.position = ultimoCheckpoint;
            rb.velocity = Vector2.zero;
        }
        else if (collision.CompareTag("SaltoExtra"))
        {
            puedeSaltoExtra = true;
            Destroy(collision.gameObject); // elimina la esfera si solo se puede usar una vez
        }
    }

    public void ActualizarCheckpoint(Vector2 nuevaPosicion)
    {
        ultimoCheckpoint = nuevaPosicion;
    }

    IEnumerator Dash()
    {
        haciendoDash = true;
        animacionDashActiva = true;
        puedeHacerDash = false;
        puedeCorrer = false;

        rb.gravityScale = 0;
        rb.velocity = new Vector2((spriteRenderer.flipX ? -1 : 1) * fuerzaDash, 0);

        animator.SetBool("Dash", true);
        animator.SetBool("falling", false);
        animator.SetBool("isRunning", false);

        yield return new WaitForSeconds(tiempoDash);

        rb.gravityScale = gravedadOriginal;
        haciendoDash = false;
        animacionDashActiva = false;
        animator.SetBool("Dash", false);

        yield return new WaitForSeconds(tiempoBloqueoCorrer);
        puedeCorrer = true;
    }

    IEnumerator RealizarSlide()
    {
        haciendoSlide = true;
        animator.SetTrigger("slide");

        float velocidadOriginal = velocidad;
        velocidad *= 1.5f;

        // Reducir el collider
        boxCollider.size = colliderSizeSlide;
        boxCollider.offset = colliderOffsetSlide;

        yield return new WaitForSeconds(tiempoSlide);

        // Restaurar collider y velocidad
        boxCollider.size = colliderSizeOriginal;
        boxCollider.offset = colliderOffsetOriginal;
        velocidad = velocidadOriginal;
        haciendoSlide = false;
    }


    IEnumerator HacerRolling()
    {
        haciendoRolling = true;
        animator.SetBool("rolling", true);

        float velocidadOriginal = velocidad;
        velocidad *= 1.2f;

        puedeHacerDash = false;
        puedeCorrer = false;

        yield return new WaitForSeconds(tiempoRolling);

        velocidad = velocidadOriginal;
        haciendoRolling = false;
        puedeHacerDash = true;
        puedeCorrer = true;

        animator.SetBool("rolling", false);
    }

    IEnumerator Levantarse()
    {
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(1.85f);

        sentado = false;
        levantandose = false;
        animator.SetBool("afk", false);
    }

    IEnumerator RealizarAtaque(int numeroAtaque)
    {
        atacando = true;

        string trigger = "atack" + numeroAtaque;
        animator.SetTrigger(trigger);
        animator.SetBool("isRunning", false);

        float retrasoActivacionHitbox = 0.5f; // Ajusta este valor según tu animación
        float duracionHitboxActiva = duracionAtaque - retrasoActivacionHitbox;

        // Esperar antes de activar la hitbox
        yield return new WaitForSeconds(retrasoActivacionHitbox);

        // Activar hitbox
        hitboxAtaque.SetActive(true);

        // Mantenerla activa por el resto de la duración del ataque
        yield return new WaitForSeconds(duracionHitboxActiva);

        // Desactivarla
        hitboxAtaque.SetActive(false);

        atacando = false;
    }


    public void RecibirDaño(int daño)
    {
        vidaActual -= daño;

        uiVida.ActualizarVida(vidaActual, vidaMaxima);

        if (vidaActual > 0)
        {
            animator.SetTrigger("hurt"); // Asegúrate de tener este trigger en el Animator
        }

        if (vidaActual <= 0)
        {
            StartCoroutine(Morir());
        }
    }

    IEnumerator Morir()
    {
        animator.SetTrigger("dead");

        rb.velocity = Vector2.zero; // Detener movimiento

        yield return new WaitForSeconds(1f); // Espera duración de la animación

        StartCoroutine(ReaparecerTrasMuerte());

    }

    IEnumerator ReaparecerTrasMuerte()
    {
        yield return new WaitForSeconds(2f); // Espera la duración de la animación de muerte (ajusta si es necesario)

        uiVida.ActualizarVida(vidaMaxima, vidaMaxima);

        transform.position = ultimoCheckpoint;
        rb.velocity = Vector2.zero;
        vidaActual = vidaMaxima;
        estaMuerto = false;

        animator.ResetTrigger("dead");
        animator.Play("idle1"); // Asegúrate de que existe esta animación o cámbiala por otra válida
    }


    private bool puedeRecibirDaño = true;
    public float tiempoInvulnerabilidad = 0.5f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("HitboxEnemigo") && puedeRecibirDaño && !estaMuerto)
        {
            RecibirDaño(1);
            StartCoroutine(InvulnerabilidadTemporal());
        }
       
    } 
    IEnumerator InvulnerabilidadTemporal()
    {
        puedeRecibirDaño = false;
        yield return new WaitForSeconds(tiempoInvulnerabilidad);
        puedeRecibirDaño = true;
    }
    
    public void Curar(int cantidad)
    {
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima)
            vidaActual = vidaMaxima;

        uiVida.ActualizarVida(vidaActual, vidaMaxima);
    }

}
