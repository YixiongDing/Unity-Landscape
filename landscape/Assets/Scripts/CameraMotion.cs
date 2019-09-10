using UnityEngine;
using System.Collections;

public class CameraMotion : MonoBehaviour
{

    private Rigidbody rd;
    private float xForce;
    private float zForce;
    private Vector3 force;

    private float pitch = 0.0F;
    private float yaw = 0.0F;

    void Start()
    {
        //Quaternion quate = Quaternion.identity;
        //quate.eulerAngles = new Vector3(50, 230, 0);
        //this.transform.rotation = quate;
    }


    void Update()
    {
        rd = this.GetComponent<Rigidbody>();
        xForce = Input.GetAxis("Horizontal");
        zForce = Input.GetAxis("Vertical");

        pitch -= Input.GetAxis("Mouse Y");
        yaw += Input.GetAxis("Mouse X");

        force = new Vector3(xForce * 20, 0.0F, zForce * 20);

        rd.transform.localEulerAngles = new Vector3(pitch * 10, yaw * 10, 0.0F);
        rd.AddRelativeForce(force);

    }
}