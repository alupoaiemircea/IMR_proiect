using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    //adaptat:https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
    //adaptat:https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=8
    public float sensitivity = 100f;
    public Transform player;

    float rotationUpDown = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        float y = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

        rotationUpDown -= y;
        rotationUpDown=Mathf.Clamp(rotationUpDown, -90f, 90f);
        transform.localRotation=Quaternion.Euler(rotationUpDown, 0f,0f);

        player.Rotate(Vector3.up * x);
    }
}
