using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;
    public LayerMask suelo;
    private Rigidbody2D rb;
    private bool enSuelo;

    public Transform detectorSuelo;
    public float radioDeteccion = 0.4f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float movimiento = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movimiento * velocidad, rb.velocity.y);

        enSuelo = Physics2D.OverlapCircle(detectorSuelo.position, radioDeteccion, suelo);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }
    }
}

