using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight: MonoBehaviour
{
    public Color color;
    public Transform center;
    public Vector3 axis = Vector3.left;
    public Vector3 desiredPosition;
    public float radius = 1000.0f;
    // public float radiusSpeed = 80f;
    public float rotationSpeed = 25.0f;

    void Update()
    {
        transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
        desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        // transform.position = Vector3.MoveTowards(transform.position, desiredPosition, radiusSpeed);
    }
    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}