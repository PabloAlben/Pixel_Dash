using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    private Rigidbody2D rb;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimiento
        float mover = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(mover * velocidad, rb.velocity.y);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }
}
