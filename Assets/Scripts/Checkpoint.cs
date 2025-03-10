using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer banderaRenderer;
    
    public Sprite banderaRoja;
    public Sprite banderaVerde; 

    private static Checkpoint ultimoCheckpoint;

    void Start()
    {
    banderaRenderer = GetComponent<SpriteRenderer>();
    if (banderaRenderer == null)
    {
        Debug.LogError("No se encontró un SpriteRenderer en el GameObject " + gameObject.name);
        return;
    }
    banderaRenderer.sprite = banderaRoja; 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador"))
        {
            if (ultimoCheckpoint != null)
            {
                ultimoCheckpoint.Desactivar(); 
            }

            Activar();
            ultimoCheckpoint = this; 

            
            collision.GetComponent<MovimientoJugador>().ActualizarCheckpoint(transform.position);
        }
    }

    void Activar()
    {
        if (banderaRenderer != null)
        {
            banderaRenderer.sprite = banderaVerde;
        }
    }

    void Desactivar()
    {
        if (banderaRenderer != null)
        {
            banderaRenderer.sprite = banderaRoja;
        }
    }
}
