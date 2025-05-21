using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nombreEscenaDestino;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador"))
        {
            StartCoroutine(CambiarEscena());
        }
    }

    private IEnumerator CambiarEscena()
    {
        // Reproducir animación o sonido aquí
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nombreEscenaDestino);
    }

}

