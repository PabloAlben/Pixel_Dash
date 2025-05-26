using System.Collections;
using UnityEngine;

public class SonidosJugador : MonoBehaviour
{
    public AudioSource audioSource; // para sonidos generales
    public AudioSource pasosSource; // NUEVO: para pasos

    public AudioClip sonidoAtaque;
    public AudioClip sonidoCurar;
    public AudioClip sonidoMorir;
    public AudioClip sonidoDash;
    public AudioClip sonidoSalto;
    public AudioClip sonidoCaminar;

    private float volumenOriginal;
    private float pitchOriginal;

    [Range(0f, 2f)]
    public float escalaSFX = 2f;

    private void Awake()
    {
        volumenOriginal = audioSource.volume;
        pitchOriginal = audioSource.pitch;
    }

    private void ReproducirConAjustes(AudioClip clip, float volumenRelativo, float pitch)
    {
        if (clip == null) return;

        float volumenGlobal = PlayerPrefs.GetFloat("volumen", 1f);
        float volumenFinal = volumenGlobal * volumenRelativo * escalaSFX;

        audioSource.volume = 1f;
        float pitchPrevio = audioSource.pitch;
        audioSource.pitch = pitch;

        audioSource.PlayOneShot(clip, Mathf.Clamp01(volumenFinal));

        audioSource.pitch = pitchPrevio;
    }

    public void ReproducirAtaque() => ReproducirConAjustes(sonidoAtaque, 1f, 1f);
    public void ReproducirCurar() => ReproducirConAjustes(sonidoCurar, 1f, 0.8f);
    public void ReproducirMorir() => ReproducirConAjustes(sonidoMorir, 1f, 0.8f);
    public void ReproducirDash() => ReproducirConAjustes(sonidoDash, 1f, 1f);
    public void ReproducirSalto() => ReproducirConAjustes(sonidoSalto, 1f, 0.6f);

    // ---------------------
    // MANEJO DE PASOS
    // ---------------------
    private Coroutine pasosCoroutine;

    public void EmpezarCaminar()
    {
        if (pasosCoroutine == null)
            pasosCoroutine = StartCoroutine(ReproducirPasos());
    }

    public void PararCaminar()
    {
        if (pasosCoroutine != null)
        {
            StopCoroutine(pasosCoroutine);
            pasosCoroutine = null;
        }

        // Detiene inmediatamente el sonido actual
        if (pasosSource.isPlaying)
            pasosSource.Stop();
    }

    private IEnumerator ReproducirPasos()
    {
        while (true)
        {
            if (sonidoCaminar != null)
            {
                pasosSource.clip = sonidoCaminar;
                pasosSource.volume = Mathf.Clamp01(PlayerPrefs.GetFloat("volumen", 1f) * 0.5f * escalaSFX);
                pasosSource.pitch = 1f;
                pasosSource.Play();
            }

            yield return new WaitForSeconds(0.4f); // ajusta el intervalo de pasos
        }
    }
}
