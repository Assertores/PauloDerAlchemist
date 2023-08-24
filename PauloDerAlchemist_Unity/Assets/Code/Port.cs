using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Port : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public Material material;
    public float Amount = 0;

    private void FixedUpdate()
    {
        text.text = material.ToString() + "(" + Extention.GetGrade(material) + ")" + "\n" + Amount.ToString("0.00");
    }
}
