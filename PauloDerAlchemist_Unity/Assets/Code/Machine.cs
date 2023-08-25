using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Machine : MonoBehaviour
{
    [SerializeField] private Transform PortIn1;
    [SerializeField] private Transform PortIn2;
    [SerializeField] private Port PortOut1;
    [SerializeField] private Port PortOut2;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float scroolFactor;

    private PipeInputHandler myFirstPipe;
    private PipeInputHandler mySecondPipe;

    bool isSet = false;

    const float MaxOutput = 50;

    public static Dictionary<Pair, Combination> combinationAtlas;
    private Combination myCombidantion;
    private float myRatio = 50;
    private bool myOn = true;

    public void Setup(PipeInputHandler aFirstPipe, PipeInputHandler aSencondPipe)
    {
        if (isSet)
        {
            print("ERROR: called Setup after Set");
            return;
        }

        myFirstPipe = aFirstPipe;
        mySecondPipe = aSencondPipe;
        myFirstPipe.Handle(PortIn1.position);
        mySecondPipe.Handle(PortIn2.position);
        text.text = myRatio.ToString();
        canvas.worldCamera = Camera.main;
    }

    public void Handle(Vector3 aTarget)
    {
        aTarget.y = 0;
        transform.position = aTarget;

        myFirstPipe.Handle(PortIn1.position);
        mySecondPipe.Handle(PortIn2.position);
    }

    public void HandleScroll(float aDelta)
    {
        transform.Rotate(new Vector3(0, aDelta * scroolFactor, 0));

        myFirstPipe.Handle(PortIn1.position);
        mySecondPipe.Handle(PortIn2.position);
    }

    public void Set()
    {
        if(isSet)
        {
            return;
        }

        var a = myFirstPipe.port.material;
        var b = mySecondPipe.port.material;
        Pair key;
        if (a < b)
        {
            key.Prime = a;
            key.Secondary = b;
        }
        else
        {
            key.Prime = b;
            key.Secondary = a;
        }
        if (!combinationAtlas.ContainsKey(key))
        {
            print(key.Prime + ":" + key.Secondary);
            return;
        }
        myCombidantion = combinationAtlas[key];

        PortOut1.material = myCombidantion.OutPrime;
        PortOut2.material = myCombidantion.OutSecondary;

        isSet = true;
    }

    public void HandleRatioChange()
    {
        myRatio = slider.value;
        text.text = myRatio.ToString("0.00");
    }

    private void FixedUpdate()
    {
        if (!isSet || !myOn)
        {
            return;
        }

        if (myFirstPipe.port.Amount == 0 || mySecondPipe.port.Amount == 0 || myRatio <= 0 || myRatio >= 100)
        {
            return;
        }
        if(PortOut1.Amount > 100 || PortOut2.Amount > 100)
        {
            return;
        }

        float Efficiency;
        float Out;
        if (myCombidantion.XMax < (myRatio / 100) * 16)
        {
            Efficiency = -0.5f * (myCombidantion.YMax + 1) * Mathf.Cos((((myRatio / 100) * 16 - 16) * Mathf.PI) / (myCombidantion.XMax - 16)) + 0.5f * (myCombidantion.YMax + 1);
        }
        else
        {
            Efficiency = -0.5f * (myCombidantion.YMax + 1) * Mathf.Cos(((myRatio / 100) * 16 * Mathf.PI) / myCombidantion.XMax) + 0.5f * (myCombidantion.YMax + 1);
        }

        Out = (Mathf.Pow(2,-(((myRatio / 100) * 16 - myCombidantion.XCurve) * ((myRatio / 100) * 16 - myCombidantion.XCurve)) / (10 * myCombidantion.DevCurve)) * myCombidantion.AmpCurve + myCombidantion.YCurve) / 2 + 8;

        var PrimeUsage = Efficiency / 16 * MaxOutput * myRatio / 100;
        var SecondaryUsage = (Efficiency / 16 * MaxOutput) - PrimeUsage;

        float usage = myFirstPipe.port.Amount / PrimeUsage;
        if (mySecondPipe.port.Amount / SecondaryUsage < usage)
        {
            usage = mySecondPipe.port.Amount / SecondaryUsage;
        }
        if (usage > 1)
        {
            usage = 1;
        }
        usage *= Time.fixedDeltaTime * 0.1f;

        PrimeUsage *= usage;
        SecondaryUsage *= usage;
        PortOut1.Amount += (Out * Efficiency) / 256 * MaxOutput * usage;
        PortOut2.Amount += ((16 - Out) * Efficiency) / 256 * MaxOutput * usage;
        myFirstPipe.port.Amount -= PrimeUsage;
        mySecondPipe.port.Amount -= SecondaryUsage;
    }
}
