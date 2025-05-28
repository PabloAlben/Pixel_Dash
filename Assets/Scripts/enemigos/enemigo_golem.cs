using System.Collections;
using UnityEngine;

public class EnemigoGolem : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public Transform puntoIzquierdo;
    public Transform puntoDerecho;
    private bool moviendoADerecha = true;

    [Header("Detección del jugador")]
    public Transform jugador;
    public float rangoDeAtaque = 2f;
    private bool jugadorEnRango;

    [Header("Ataque")]
    public float tiempoEntreAtaques = 2f;
    private float tiempoDesdeUltimoAtaque = 0f;
    private bool estaAtacando = false;
    public float tiempoActivacionGolpe = 0.9f;
    public float duracionHitbox = 0.2f;
    private float tiempoAtaqueActual = 0f;
    private bool golpeEjecutado = false;
    [SerializeField] private GameObject hitboxAtaque;

    [Header("Referencias")]
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Vida")]
    public int vida = 5;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        jugadorEnRango = Vector2.Distance(transform.position, jugador.position) <= rangoDeAtaque;

        if (estaAtacando)
        {
            tiempoAtaqueActual += Time.deltaTime;
            return;
        }

        tiempoDesdeUltimoAtaque += Time.deltaTime;

        if (jugadorEnRango && tiempoDesdeUltimoAtaque >= tiempoEntreAtaques)
        {
            Atacar();
        }
        else if (!jugadorEnRango)
        {
            Patrullar();
        }
        else
        {
            animator.SetBool("isWalking", false);
            rb.velocity = Vector2.zero;
        }
    }

    private void Patrullar()
    {
        animator.SetBool("isWalking", true);

        float direccion = moviendoADerecha ? 1f : -1f;
        rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);

        if (moviendoADerecha && transform.position.x >= puntoDerecho.position.x)
            moviendoADerecha = false;
        else if (!moviendoADerecha && transform.position.x <= puntoIzquierdo.position.x)
            moviendoADerecha = true;

        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * (moviendoADerecha ? 1 : -1);
        transform.localScale = escala;
    }

    private void Atacar()
    {
        if (estaAtacando) return;

        estaAtacando = true;
        golpeEjecutado = false;
        tiempoDesdeUltimoAtaque = 0f;
        rb.velocity = Vector2.zero;

        animator.SetBool("isWalking", false);
        animator.SetTrigger("isSlashing");

        // Girar hacia el jugador
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * (jugador.position.x > transform.position.x ? 1 : -1);
        transform.localScale = escala;

        StartCoroutine(EjecutarAtaque());
    }

    private System.Collections.IEnumerator EjecutarAtaque()
    {
        Debug.Log("Inicia ataque");

        yield return new WaitForSeconds(tiempoActivacionGolpe);

        if (!golpeEjecutado)
        {
            hitboxAtaque.SetActive(false);
            hitboxAtaque.SetActive(true);
            hitboxAtaque.transform.position += new Vector3(0.01f, 0, 0);
            golpeEjecutado = true;
            Debug.Log("Hitbox activada");
        }

        yield return new WaitForSeconds(duracionHitbox);

        hitboxAtaque.transform.position -= new Vector3(0.01f, 0, 0);
        hitboxAtaque.SetActive(false);
        Debug.Log("Hitbox desactivada");

        yield return new WaitForSeconds(0.2f);

        estaAtacando = false;
        Debug.Log("Fin del ataque");
    }


    public void RecibirDaño(int daño)
    {

        if (vida > 0)
        {
            vida -= daño;
            animator.SetTrigger("Hit");
        }
        else if (vida <= 0)
            {
                Morir();
            }
    }

    public void Morir()
    {
        animator.SetTrigger("Dead");
        this.enabled = false;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
    }

    private float tiempoUltimoDaño;
    public float cooldownDaño = 0.5f;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("HitboxJugador") && Time.time - tiempoUltimoDaño > cooldownDaño)
        {
            RecibirDaño(1);
            tiempoUltimoDaño = Time.time;
        }
    }
}
