using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltoExtra : MonoBehaviour
{
    public Sprite spriteOriginal;
    public Sprite spriteDesactivado;

    private SpriteRenderer sr;
    private Collider2D col;
    private bool activo = true;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void ActivarEfecto()
    {
        if (!activo) return;

        // Aquí puedes notificar al jugador si quieres, pero esto lo llamará el jugador

        // Cambiar a sprite desactivado
        sr.sprite = spriteDesactivado;
        col.enabled = false;
        activo = false;

        // Reactivar tras 10 segundos
        StartCoroutine(ReactivarDespuesDeTiempo(4f));
    }

    private IEnumerator ReactivarDespuesDeTiempo(float segundos)
    {
        yield return new WaitForSeconds(segundos);

        sr.sprite = spriteOriginal;
        col.enabled = true;
        activo = true;
    }
}

