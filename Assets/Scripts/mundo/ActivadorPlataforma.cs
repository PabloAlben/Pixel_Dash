using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorPlataforma : MonoBehaviour
{
    public PlataformaAlterna plataforma;
    public KeyCode teclaActivacion = KeyCode.E;

    private bool jugadorDentro = false;

    private void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(teclaActivacion))
        {
            plataforma.ActivarMovimientoManual();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            jugadorDentro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            jugadorDentro = false;
        }
    }
}

