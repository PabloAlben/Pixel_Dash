using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSlime : MonoBehaviour
{
    private bool yaGolpeado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!yaGolpeado && collision.CompareTag("Player"))
        {
            collision.GetComponent<MovimientoJugador>()?.RecibirDaño(1); // Ajusta esto según tu sistema de daño
            yaGolpeado = true;
        }
    }

    private void OnEnable()
    {
        yaGolpeado = false; // Resetear al volver a activar la hitbox
    }
}

