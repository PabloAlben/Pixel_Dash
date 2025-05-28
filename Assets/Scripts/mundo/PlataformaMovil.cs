using System.Collections;
using UnityEngine;

public class PlataformaAlterna : MonoBehaviour
{
    [Header("Puntos A y B")]
    public Transform puntoA;
    public Transform puntoB;

    [Header("Configuración")]
    public float velocidad = 2f;
    public bool modoManual = false;

    private Transform destinoActual;
    private bool enMovimiento = false;

    private void Start()
    {
        destinoActual = puntoB;
        if (!modoManual)
        {
            StartCoroutine(MovimientoAutomatico());
        }
    }

    private IEnumerator MovimientoAutomatico()
    {
        while (!modoManual)
        {
            yield return MoverHacia(destinoActual);
            destinoActual = (destinoActual == puntoA) ? puntoB : puntoA;
            yield return new WaitForSeconds(1f);
        }
    }

    public void ActivarMovimientoManual()
    {
        if (modoManual && !enMovimiento)
        {
            StartCoroutine(MoverHacia(destinoActual));
            destinoActual = (destinoActual == puntoA) ? puntoB : puntoA;
        }
    }

    private IEnumerator MoverHacia(Transform destino)
    {
        enMovimiento = true;

        while ((transform.position - destino.position).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);
            yield return null;
        }

        transform.position = destino.position;
        enMovimiento = false;
    }
}
