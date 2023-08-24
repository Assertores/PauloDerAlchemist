using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeInputHandler : MonoBehaviour
{
    [SerializeField] private GameObject end;
    [SerializeField] private GameObject endTop;
    [SerializeField] private GameObject over;
    [SerializeField] private GameObject down;

    public Port port;

    public void Handle(Vector3 aTarget)
    {
        aTarget -= transform.position;
        aTarget.y = 0;

        end.transform.localPosition = new Vector3(aTarget.x, end.transform.localPosition.y, aTarget.z);
        endTop.transform.localPosition = new Vector3(aTarget.x, endTop.transform.localPosition.y, aTarget.z);
        down.transform.localPosition = new Vector3(aTarget.x, down.transform.localPosition.y, aTarget.z);

        over.transform.localPosition = new Vector3(aTarget.x / 2, over.transform.localPosition.y, aTarget.z / 2);
        over.transform.localScale = new Vector3(1, 1, aTarget.magnitude);
        over.transform.localRotation = Quaternion.LookRotation(aTarget);
    }
}
