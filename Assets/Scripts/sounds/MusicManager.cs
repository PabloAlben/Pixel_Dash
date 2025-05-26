using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instancia;

    private void Start()
    {
        GetComponent<AudioSource>().Play();
    }


    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // No destruir entre escenas
        }
        else
        {
            Destroy(gameObject); // Si ya hay uno, se elimina el duplicado
        }
    }
}
