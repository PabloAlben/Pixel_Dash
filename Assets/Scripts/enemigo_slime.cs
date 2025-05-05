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

    [Header("Referencias")]
    private Animator animator;
    private Rigidbody2D rb;
    private bool estaAtacando = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        jugadorEnRango = Vector2.Distance(transform.position, jugador.position) <= rangoDeAtaque;

        AnimatorStateInfo estadoActual = animator.GetCurrentAnimatorStateInfo(0);

        if (estaAtacando)
        {
            // Si ya no está en la animación de ataque, se terminó el ataque
            if (!estadoActual.IsName("Slash"))
            {
                estaAtacando = false;
            }
            return;
        }

        if (jugadorEnRango)
        {
            Atacar();
        }
        else
        {
            Patrullar();
        }
    }



    private void Patrullar()
    {
        animator.SetBool("isWiggling", true);
        animator.SetBool("isIdle", false);

        // Movimiento
        float direccion = moviendoADerecha ? 1f : -1f;
        rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);

        // Flip y cambio de dirección
       if (moviendoADerecha && transform.position.x >= puntoDerecho.position.x)
        {
            moviendoADerecha = false;
        }
        else if (!moviendoADerecha && transform.position.x <= puntoIzquierdo.position.x)
        {
            moviendoADerecha = true;
        }

        // Aplicar el flip después de cambiar la dirección
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * (moviendoADerecha ? 1 : -1);
        transform.localScale = escala;
    }

    private void Atacar()
    {
        animator.SetBool("isWiggling", false);
        animator.SetBool("isIdle", true);

        AnimatorStateInfo estadoActual = animator.GetCurrentAnimatorStateInfo(0);
        if (!estadoActual.IsName("Slash"))
        {
            // Girar hacia el jugador solo si va a iniciar el ataque
            Vector3 escalaActual = transform.localScale;
            float escalaX = Mathf.Abs(escalaActual.x);
            if (jugador.position.x > transform.position.x)
                escalaActual.x = escalaX;
            else
                escalaActual.x = -escalaX;
            transform.localScale = escalaActual;

            animator.SetTrigger("isSlashing");
            estaAtacando = true;
            rb.velocity = Vector2.zero;
        }
    }

    public void RecibirDaño()
    {
        animator.SetTrigger("isHit");
        // Aquí puedes añadir lógica de vida
    }

    public void Morir()
    {
        animator.SetTrigger("isDead");
        this.enabled = false;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
    }

    public void FinAtaque()
    {
        estaAtacando = false;
    }



}


