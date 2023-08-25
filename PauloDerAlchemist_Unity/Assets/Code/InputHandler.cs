using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private GameObject machinePrefab;
    [SerializeField] private CameraMovement cameraHandler;
    private PipeInputHandler pipe = null;
    private Machine machine = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pipe)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Port"))
                {
                    pipe.Handle(hit.collider.transform.position);
                }
                else
                {
                    pipe.Handle(hit.point);
                }
            }
        }
        if (machine)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                machine.Handle(hit.point);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (machine)
                {
                    machine.Set();
                    machine = null;
                }
                //else if(hit.collider.CompareTag("Machine"))
                //{
                //    machine = hit.collider.transform.parent.GetComponent<Machine>();
                //}
                if (hit.collider.CompareTag("Port"))
                {
                    if (!pipe)
                    {
                        var instance = Instantiate(pipePrefab);
                        instance.transform.position = hit.collider.transform.position;
                        pipe = instance.GetComponent<PipeInputHandler>();

                        pipe.port = hit.collider.transform.parent.GetComponent<Port>();
                    }
                    else
                    {
                        ConnectPorts(hit.point, hit.collider.transform);
                        pipe = null;
                    }
                }
            }
        }
    }

    private void ConnectPorts(Vector3 aTarget, Transform aPortPosition)
    {
        var instance = Instantiate(machinePrefab);
        machine = instance.GetComponent<Machine>();

        var secondPipe = Instantiate(pipePrefab);
        secondPipe.transform.position = aPortPosition.position;
        secondPipe.GetComponent<PipeInputHandler>().port = aPortPosition.parent.GetComponent<Port>();

        machine.Setup(pipe, secondPipe.GetComponent<PipeInputHandler>());
        machine.Handle(aTarget);
    }
}
