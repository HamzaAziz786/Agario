using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionScript : MonoBehaviour
{


    public float distanceFromCamera = 10f;
    public float movementSpeed = .3f;

    void LateUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distanceFromCamera;

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        targetPosition.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed /** Time.deltaTime*/);
    }



}
