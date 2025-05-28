using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nombreEscenaDestino;

    private AudioSource audioSource;

    [Range(0f, 2f)]
    [SerializeField] private float volumenRelativo = 1f; // Puedes ajustar este valor en el Inspector

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jugador"))
        {
            StartCoroutine(CambiarEscena());
        }
    }

    private IEnumerator CambiarEscena()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            float volumenGlobal = PlayerPrefs.GetFloat("volumen", 1f);
            audioSource.volume = Mathf.Clamp01(volumenGlobal * volumenRelativo);
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length); // espera a que termine
        }
        else
        {
            yield return new WaitForSeconds(1f); // espera por defecto
        }

        SceneManager.LoadScene(nombreEscenaDestino);
    }
}
