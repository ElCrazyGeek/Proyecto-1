using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Transform del jugador
    public Vector3 offset = new Vector3(0, 0, -10); // Offset base de la cámara (Z = -10 para 2D)
    public float smoothSpeed = 0.125f; // Suavidad del seguimiento
    public float lookAheadDistance = 5f; // Distancia adicional al presionar Shift
    public float lookAheadSpeed = 0.2f; // Velocidad del desplazamiento al mirar más allá

    private Vector3 currentOffset;

    void Start()
    {
        currentOffset = offset; // Inicializar offset
        if (player == null)
        {
            Debug.LogError("No se ha asignado el Transform del jugador en CameraFollow.");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Posición deseada base (sigue al jugador)
        Vector3 desiredPosition = player.position + currentOffset;

        // Desplazamiento adicional si se presiona Shift
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // Obtener la posición del ratón en el mundo
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            // Dirección desde el jugador al ratón
            Vector3 lookDirection = (mousePos - player.position).normalized;
            // Añadir desplazamiento en la dirección del ratón
            desiredPosition += lookDirection * lookAheadDistance;
        }

        // Suavizar el movimiento de la cámara
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Asegurar que la cámara no rote
        transform.rotation = Quaternion.identity;
    }
}