using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotation;

    void Update()
    {
        var trueSpeed = speed * Time.deltaTime;
        var trueRotation = rotation * Time.deltaTime; 

        // TODO: fix translation if camera is rotated
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, trueSpeed), Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-trueSpeed, 0, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -trueSpeed), Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(trueSpeed, 0, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, -trueRotation, 0));
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, trueRotation, 0));
        }
    }
}
