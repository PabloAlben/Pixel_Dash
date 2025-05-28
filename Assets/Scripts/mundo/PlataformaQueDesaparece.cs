using System.Collections;
using UnityEngine;

public class PlataformaQueDesaparece : MonoBehaviour
{
    public float tiempoParaDesaparecer = 0.5f;
    public float tiempoParaReaparecer = 2f;
    public float retardoInicial = 2f;

    private Collider2D col;
    private SpriteRenderer sr;
    private bool primeraVez = true;
    private bool modoAutomatico = false;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        if (retardoInicial > 0)
        {
            modoAutomatico = true;
            StartCoroutine(DesaparecerEnBucleConRetardo());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!modoAutomatico && collision.collider.CompareTag("Jugador"))
        {
            StartCoroutine(DesaparecerTemporalmente());
        }
    }

    private IEnumerator DesaparecerEnBucleConRetardo()
    {
        if (primeraVez)
        {
            yield return new WaitForSeconds(retardoInicial);
            primeraVez = false;
        }

        while (true)
        {
            yield return new WaitForSeconds(tiempoParaDesaparecer);

            col.enabled = false;
            sr.enabled = false;

            yield return new WaitForSeconds(tiempoParaReaparecer);

            col.enabled = true;
            sr.enabled = true;
        }
    }

    private IEnumerator DesaparecerTemporalmente()
    {
        if (primeraVez)
        {
            yield return new WaitForSeconds(retardoInicial);
            primeraVez = false;
        }

        yield return new WaitForSeconds(tiempoParaDesaparecer);

        col.enabled = false;
        sr.enabled = false;

        yield return new WaitForSeconds(tiempoParaReaparecer);

        col.enabled = true;
        sr.enabled = true;
    }
}
