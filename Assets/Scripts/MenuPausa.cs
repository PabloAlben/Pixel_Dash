using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject menuPausaUI;
    private bool juegoPausado = false;

    [Header("Sonido")]
    public Sprite sonidoActivadoSprite;
    public Sprite sonidoDesactivadoSprite;
    public Image botonSonidoImage;
    private bool sonidoActivo = true;

    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
                Reanudar();
            else
                Pausar();
        }
    }

    public void Reanudar()
    {
        menuPausaUI.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    void Pausar()
    {
        menuPausaUI.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void SalirJuego()
    {
       Time.timeScale = 1f;
       SceneManager.LoadScene("MainMenu");
    }

    // 🔊 Función que puedes vincular al botón de sonido
    public void ToggleSonido()
    {
        sonidoActivo = !sonidoActivo;

        AudioListener.pause = !sonidoActivo;

        // Cambiar sprite del botón
        if (botonSonidoImage != null)
        {
            botonSonidoImage.sprite = sonidoActivo ? sonidoActivadoSprite : sonidoDesactivadoSprite;
        }
    }
}
