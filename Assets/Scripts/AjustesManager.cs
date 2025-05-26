using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AjustesManager : MonoBehaviour
{
    [Header("Volumen")]
    public Slider sliderVolumen;
    private AudioSource audioGlobal;

    [Header("Pantalla")]
    public Toggle togglePantallaCompleta;

    [Header("VSync")]
    public Toggle toggleVsync;

    [Header("Menús")]
    public GameObject menuAjustes;
    public GameObject menuInicio;

    [Range(0f, 1f)]
    public float escalaMusica = 0.3f;


    void Start()
    {
        BuscarAudioGlobal(); // buscar al iniciar
        CargarAjustes();
    }

    void BuscarAudioGlobal()
    {
        // Busca el AudioSource del objeto MusicManager (marcado como DontDestroyOnLoad)
        GameObject musicObj = GameObject.FindWithTag("Music");
        if (musicObj != null)
        {
            audioGlobal = musicObj.GetComponent<AudioSource>();
        }
    }

    void CargarAjustes()
    {
        float volumenGuardado = PlayerPrefs.GetFloat("volumen", 1f);
        sliderVolumen.value = volumenGuardado;

        CambiarVolumen(); // Esto ya aplica el volumen con escalaMusica

        bool pantallaCompletaGuardada = PlayerPrefs.GetInt("pantallaCompleta", 1) == 1;
        bool vsyncGuardado = PlayerPrefs.GetInt("vsync", 0) == 1;

        togglePantallaCompleta.isOn = pantallaCompletaGuardada;
        Screen.fullScreen = pantallaCompletaGuardada;

        toggleVsync.isOn = vsyncGuardado;
        QualitySettings.vSyncCount = vsyncGuardado ? 1 : 0;
    }


    public void CambiarVolumen()
    {
        if (audioGlobal == null)
            BuscarAudioGlobal();

        if (audioGlobal != null)
            audioGlobal.volume = sliderVolumen.value * escalaMusica;

        PlayerPrefs.SetFloat("volumen", sliderVolumen.value);
    }


    public void CambiarPantallaCompleta()
    {
        Screen.fullScreen = togglePantallaCompleta.isOn;
    }

    public void CambiarVsync()
    {
        QualitySettings.vSyncCount = toggleVsync.isOn ? 1 : 0;
    }

    public void GuardarAjustes()
    {
        PlayerPrefs.SetFloat("volumen", sliderVolumen.value);
        PlayerPrefs.SetInt("pantallaCompleta", togglePantallaCompleta.isOn ? 1 : 0);
        PlayerPrefs.SetInt("vsync", toggleVsync.isOn ? 1 : 0);
        PlayerPrefs.Save();

        menuAjustes.SetActive(false);
        menuInicio.SetActive(true);
    }

    public void ResetearAjustes()
    {
        sliderVolumen.value = 1f;
        togglePantallaCompleta.isOn = true;
        toggleVsync.isOn = false;

        CambiarVolumen();
        CambiarPantallaCompleta();
        CambiarVsync();
    }
}
