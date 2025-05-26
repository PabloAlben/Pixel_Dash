using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaJugadorUI : MonoBehaviour
{
    public Image[] corazones; // Asigna los iconos en el Inspector
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    public void ActualizarVida(int vidaActual, int vidaMaxima)
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vidaActual)
                corazones[i].sprite = corazonLleno;
            else
                corazones[i].sprite = corazonVacio;

            corazones[i].enabled = i < vidaMaxima;
        }
    }
}
