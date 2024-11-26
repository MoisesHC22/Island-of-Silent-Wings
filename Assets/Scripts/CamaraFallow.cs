using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFallow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    public float minX; // Límite izquierdo
    public float maxX;
    public float minY;
    public float maxY;

    private void LateUpdate()
    {
        if (target != null) 
        {
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);

            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);


            // Ajusta la posición de la cámara con suavidad
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
