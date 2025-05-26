using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AjustesManager : MonoBehaviour
{
    [Header("Volumen")]
    public Slider sliderVolumen;
    public AudioSource audioGlobal;

    [Header("Pantalla")]
    public Toggle togglePantallaCompleta;

    [Header("VSync")]
    public Toggle toggleVsync;

    [Header("Menús")]
    public GameObject menuAjustes;
    public GameObject menuInicio;

    void Start()
    {
        CargarAjustes();
    }

    void CargarAjustes()
    {
        float volumenGuardado = PlayerPrefs.GetFloat("volumen", 1f);
        bool pantallaCompletaGuardada = PlayerPrefs.GetInt("pantallaCompleta", 1) == 1;
        bool vsyncGuardado = PlayerPrefs.GetInt("vsync", 0) == 1;

        sliderVolumen.value = volumenGuardado;
        audioGlobal.volume = volumenGuardado;

        togglePantallaCompleta.isOn = pantallaCompletaGuardada;
        Screen.fullScreen = pantallaCompletaGuardada;

        toggleVsync.isOn = vsyncGuardado;
        QualitySettings.vSyncCount = vsyncGuardado ? 1 : 0;
    }

    public void CambiarVolumen()
    {
        audioGlobal.volume = sliderVolumen.value;
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

        // Volver al menú de inicio
        menuAjustes.SetActive(false);
        menuInicio.SetActive(true);
    }

    public void ResetearAjustes()
    {
        // Ajustes por defecto
        sliderVolumen.value = 1f;
        togglePantallaCompleta.isOn = true;
        toggleVsync.isOn = false;

        // Aplicar visualmente
        CambiarVolumen();
        CambiarPantallaCompleta();
        CambiarVsync();
    }
}

