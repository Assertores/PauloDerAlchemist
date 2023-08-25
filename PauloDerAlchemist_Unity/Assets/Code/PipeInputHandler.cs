using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeInputHandler : MonoBehaviour
{
    [SerializeField] private GameObject end;
    [SerializeField] private GameObject endTop;
    [SerializeField] private GameObject over;
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject up;
    [SerializeField] private GameObject startTop;
    [SerializeField] private float scroolFactor;

    [HideInInspector] public Port port;

    public void Handle(Vector3 aTarget)
    {
        aTarget -= transform.position;
        aTarget.y = 0;

        end.transform.localPosition = new Vector3(aTarget.x, end.transform.localPosition.y, aTarget.z);
        endTop.transform.localPosition = new Vector3(aTarget.x, endTop.transform.localPosition.y, aTarget.z);
        down.transform.localPosition = new Vector3(aTarget.x, down.transform.localPosition.y, aTarget.z);

        over.transform.localPosition = new Vector3(aTarget.x / 2, over.transform.localPosition.y, aTarget.z / 2);
        over.transform.localScale = new Vector3(1, 1, aTarget.magnitude);
        if (aTarget.magnitude > 0)
        {
            over.transform.localRotation = Quaternion.LookRotation(aTarget);
        }
    }

    public void HandleScroll(float aDelta)
    {
        var translation = new Vector3(0, aDelta * scroolFactor, 0);
        up.transform.localScale += translation;
        up.transform.localPosition += translation / 2;
        down.transform.localScale += translation;
        down.transform.localPosition += translation / 2;

        startTop.transform.localPosition += translation;
        over.transform.localPosition += translation;
        endTop.transform.localPosition += translation;
    }
}
