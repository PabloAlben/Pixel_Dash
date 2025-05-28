using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Jugador"))
        {
            MovimientoJugador jugador = col.GetComponent<MovimientoJugador>();

            if (jugador != null)
            {
                col.transform.position = jugador.ultimoCheckpoint; // <--- usamos directamente el Vector2
            }
        }
    }
}
