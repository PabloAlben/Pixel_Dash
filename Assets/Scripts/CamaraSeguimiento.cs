using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    public Transform jugador;
    public Vector3 offset;

    void Update()
    {
        if (jugador != null)
        {
            transform.position = new Vector3(jugador.position.x + offset.x, jugador.position.y + offset.y, transform.position.z);
        }
    }
}
