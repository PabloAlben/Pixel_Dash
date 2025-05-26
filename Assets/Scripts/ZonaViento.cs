using UnityEngine;

public class ZonaViento : MonoBehaviour
{
    public Vector2 direccionViento = new Vector2(1f, 0f); // Viento hacia la derecha
    public float fuerzaViento = 200f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            MovimientoJugador jugador = other.GetComponent<MovimientoJugador>();
            if (jugador != null && !jugador.haciendoDash)
            {
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(direccionViento.normalized * fuerzaViento * Time.deltaTime, ForceMode2D.Force);
                }
            }
        }
    }

    public ParticleSystem particulas;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            if (particulas != null) particulas.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            if (particulas != null) particulas.Stop();
        }
    }

}
