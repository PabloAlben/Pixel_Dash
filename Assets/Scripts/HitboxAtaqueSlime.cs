using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxAtaqueSlime : MonoBehaviour
{
    public int daño = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            MovimientoJugador jugador = other.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.RecibirDaño(daño);
            }
        }
    }
}

