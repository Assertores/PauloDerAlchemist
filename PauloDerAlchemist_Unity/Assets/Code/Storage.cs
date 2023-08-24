using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] Port port;
    [SerializeField] float supply;
    [SerializeField] float max;

    private void FixedUpdate()
    {
        port.Amount += supply;
        if(port.Amount > max)
        {
            port.Amount = max;
        }
    }
}
