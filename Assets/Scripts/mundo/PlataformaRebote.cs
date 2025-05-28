using UnityEngine;

public class PlataformaRebote : MonoBehaviour
{
    public float fuerzaVertical = 20f;
    public float fuerzaLateral = 20f;

   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                float direccionLateral = rb.velocity.x >= 0 ? 1f : -1f;
                rb.velocity = new Vector2(direccionLateral * fuerzaLateral, fuerzaVertical);

                // Llamar al script de movimiento para que resetee animaciones
                MovimientoJugador movimiento = other.GetComponent<MovimientoJugador>();
                if (movimiento != null)
                {
                    movimiento.OnBounce();
                }
            }
        }
    }

}
