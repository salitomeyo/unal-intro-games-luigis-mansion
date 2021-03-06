using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{   
    [SerializeField]
    [Range(0, 100)]
    [Tooltip("Distancia maxima de vision en la que puede detectar al jugador")]
    private float distance;

    [SerializeField]
    [Range(0, 360)]
    [Tooltip("Angulo de vision en el que puede detectar al jugador")]
    private float angle;

    [SerializeField]
    [Tooltip("Layer en la que se encuentran los objetos que quiero detectar")]
    private LayerMask targetLayer;

    [SerializeField]
    [Tooltip("Layer en la que se encuentran los objetos que quiero como obstaculos en la deteccion")]
    private LayerMask obstacleLayer;

    private Collider detectedTarget;
    private Vector3 directionToCollider;
    private Vector3 directionFromPlayer;
    private float distanceToCollider = 100;

    // Update is called once per frame
    void Update()
    {
        detectedTarget = null;
        distanceToCollider = 100;
        //Lista de todos los colliders que estan dentro del rango de deteccion
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, targetLayer);

        detectTarget(colliders);

        if (detectedTarget != null)
        {
            distanceToCollider = Mathf.Sqrt((Mathf.Pow((transform.position.x-detectedTarget.bounds.center.x), 2f))+(Mathf.Pow((transform.position.z-detectedTarget.bounds.center.z), 2f)));
        }
    }

    private void detectTarget(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            //vector direccion entre el collider y el enemigo transform
            directionToCollider = Vector3.Normalize(transform.position - collider.bounds.center);
            directionToCollider = Vector3.Normalize(collider.bounds.center - transform.position);

            float angleToCollider = Vector3.Angle(directionToCollider, transform.forward);

            //si el angulo es menor que el de vision
            if (angleToCollider <= angle)
            {
                //verificacion nada bloquea la vista del enemigo
                if(!Physics.Linecast(transform.position, collider.bounds.center, obstacleLayer))
                {
                    detectedTarget = collider;
                    break;
                }
            }
        }
    }

    public void FaceTarget()
    {
        // Vector3 lookPos = destination - transform.position;
        directionToCollider.y = 0;
        Quaternion rotation = Quaternion.LookRotation(directionToCollider);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);  
    }

    public Vector3 GetDirection()
    {
        return directionToCollider;
    }

    public Vector3 GetDirectionFromPlayer()
    {
        return directionToCollider;
    }

    public float GetDistance()
    {
        return distanceToCollider;
    }

    public Collider getTarget()
    {
        return detectedTarget;
    }

    //Nos permite ver de forma grafica el rango de deteccion de los enemigos
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        //Dibuja una esfera basada en el distance definido
        Gizmos.DrawWireSphere(transform.position, distance);

        //Dibuja dos lineas rectas dentro de la esfera que corresponden al angulo definido
        Vector3 rightDir = Quaternion.Euler(0, angle, 0)*transform.forward;
        Gizmos.DrawRay(transform.position, rightDir*distance);

        Vector3 leftDir = Quaternion.Euler(0, -angle, 0)*transform.forward;
        Gizmos.DrawRay(transform.position, leftDir*distance);

    }
}
