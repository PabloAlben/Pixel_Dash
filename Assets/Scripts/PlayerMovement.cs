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
       

        enSuelo = Physics2D.OverlapCircle(detectorSuelo.position, radioDeteccion, suelo);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }

        
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    void FixedUpdate()
    {
         float movimiento = Input.GetAxis("Horizontal");

        
        float velocidadActual = enSuelo ? velocidad : Mathf.Max(velocidad, velocidad * 1.2f);
        rb.velocity = new Vector2(movimiento * velocidadActual * Time.deltaTime * 60, rb.velocity.y);
        
        if (!enSuelo && rb.velocity.y < 0)
        {
            rb.gravityScale = 3f; 
        }
        else
        {
            rb.gravityScale = 2f; 
        }
    }
}

