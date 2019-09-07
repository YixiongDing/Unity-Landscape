using UnityEngine;
using System.Collections;

public class CameraMotion : MonoBehaviour
{
    float speed = 100f;                 // Speed of camera
    float camSens = .30f;               // Pitch and yaw sensitivity
    float roll = 0f;                    // Absolute value of roll applied
    Vector3 pos = new Vector3(0, 0, 0); // New position from K/B input

    /** Previous heading is given an initial direction */
    private Vector3 prevHead = new Vector3(Screen.width / 2, Screen.height / 2, 0);

    public Transform model;
    private const float default_distance = 500f;


    void Start()
    {
        // 旋转归零
        transform.rotation = Quaternion.identity;
        // 初始位置是模型
        Vector3 position = model.position;
        position.z -= default_distance;
        transform.position = position;
    }


    void Update()
    {
        /** Parse mouse headings */
        prevHead = Input.mousePosition - prevHead;
        prevHead = new Vector3(-prevHead.y * camSens, prevHead.x * camSens,
                                roll);
        prevHead = new Vector3(transform.eulerAngles.x + prevHead.x,
                                transform.eulerAngles.y + prevHead.y, roll);
        transform.eulerAngles = prevHead;
        prevHead = Input.mousePosition;

        /** Parse keyboard movements */
        pos = getDirection();
        pos = pos * speed;
        pos = pos * Time.deltaTime;
        transform.Translate(pos);

    }

    /** Keybindings for keyboard controls */
    private Vector3 getDirection()
    {
        Vector3 vel = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            vel += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            vel += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            vel += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            vel += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            roll += 0.5f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            roll += -0.5f;
        }
        return vel;
    }

    /** Collision response */
    void OnTriggerEnter(Collider col)
    {
        transform.Translate(-pos * 5);
    }
}