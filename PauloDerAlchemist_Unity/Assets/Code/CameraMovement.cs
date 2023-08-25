using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float zoomOutSlowDown;
    [SerializeField] float rotation;
    [SerializeField] Camera myCamera;

    void Update()
    {
        var trueSpeed = speed * Time.deltaTime * myCamera.transform.position.y * zoomOutSlowDown;
        var trueRotation = rotation * Time.deltaTime;

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

        HandleScroll(Input.mouseScrollDelta.y);
    }

    void HandleScroll(float aDelta)
    {
        var newPosition = myCamera.transform.position + (myCamera.transform.forward * aDelta);
        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        myCamera.transform.position = new Vector3(myCamera.transform.position.x, newPosition.y, myCamera.transform.position.z);
    }
}
