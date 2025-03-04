using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Jugador"))
        {
            col.transform.position = new Vector3(0, -1.4f, 0);
        }
    }
}

