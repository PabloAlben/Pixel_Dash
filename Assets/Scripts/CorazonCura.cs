using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorazonCura : MonoBehaviour
{
    public int cantidadCura = 1; // Vida que recupera

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador"))
        {
            // Accedemos al script del jugador
            MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.Curar(cantidadCura);
            }

            Destroy(gameObject); // Eliminar el corazón tras cogerlo
        }
    }
}
