using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoSlime : MonoBehaviour
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
    public int vida = 3;

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
            animator.SetBool("isWiggling", false);
            animator.SetBool("isIdle", true);
            rb.velocity = Vector2.zero;
        }
    }

    private void Patrullar()
    {
        animator.SetBool("isWiggling", true);
        animator.SetBool("isIdle", false);

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
        animator.SetBool("isWiggling", false);
        animator.SetBool("isIdle", true);

        if (estaAtacando) return;

        estaAtacando = true;
        golpeEjecutado = false;
        tiempoDesdeUltimoAtaque = 0f;
        rb.velocity = Vector2.zero;

        animator.SetTrigger("isSlashing");

        // Girar hacia el jugador al atacar
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * (jugador.position.x > transform.position.x ? 1 : -1);
        transform.localScale = escala;

        // Activar el golpe tras un retardo
        Invoke(nameof(ActivarGolpe), tiempoActivacionGolpe);

        float duracionTotalDelAtaque = tiempoActivacionGolpe + duracionHitbox + 0.2f;
        Invoke(nameof(FinAtaque), duracionTotalDelAtaque);
    }

    private void ActivarGolpe()
    {
        
        if (!golpeEjecutado)
        {
        
            hitboxAtaque.SetActive(true);
            hitboxAtaque.transform.position += new Vector3(0.01f, 0, 0);
            golpeEjecutado = true;

            // Desactivamos la hitbox tras su duración
            Invoke(nameof(DesactivarHitbox), duracionHitbox);
        }
    }


    private void FinAtaque()
    {
        estaAtacando = false;
        
    }

    private void DesactivarHitbox()
    {
        hitboxAtaque.transform.position -= new Vector3(0.01f, 0, 0);
        hitboxAtaque.SetActive(false);
    }

    public void RecibirDaño(int daño)
    {
        vida -= daño;
        animator.SetTrigger("isHit");

        if (vida <= 0)
        {
            Morir();
        }
    }

 public void Morir()
    {
        animator.SetTrigger("isDead");
        this.enabled = false;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
    
        Destroy(gameObject, 1f);
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
