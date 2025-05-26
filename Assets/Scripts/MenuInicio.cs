using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInicio : MonoBehaviour
{
    public Image buttonImage;             // Image del botón
    public Sprite soundOnSprite;          // Icono cuando el sonido está activado
    public Sprite soundOffSprite;         // Icono cuando el sonido está desactivado

    private bool isMuted = false;

    [Header("Menús")]
    public GameObject menuAjustes;
    public GameObject menuInicio;

    public void Start()
    {
        // Recuperar estado anterior (opcional)
        isMuted = PlayerPrefs.GetInt("SoundMuted", 0) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateButtonSprite();
    }

    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;

        // Guardar el estado
        PlayerPrefs.SetInt("SoundMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateButtonSprite();
    }

    private void UpdateButtonSprite()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }

    public void ajustes()
    { 
        menuInicio.SetActive(false);
        menuAjustes.SetActive(true);

    }

}
