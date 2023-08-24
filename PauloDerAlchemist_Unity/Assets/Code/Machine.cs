using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Material
{
    Feuer = 0, Wasser = 1, Erde = 2, Luft = 3,
    Helium = 4, Ozon = 5, Kohlenstoff = 6,
    Methanol = 7, Propan = 8, Ethan = 9, Acetaldehyd = 10,
    Polyvinylchlorid = 11, Polytetrafluorethylen = 12, Siliciumdioxid = 13,
    Plastik = 14, Glas = 15,
    Stahl = 16, Aluminium = 17, Messing = 18,
    Plutonium = 19, Radon = 20, Uran = 21, Fermium = 22,
    Silicium = 23, Wolfram = 24, Silber = 25,
    Quecksilber = 26, Platin = 27,
    Gold = 28
};

public struct Combination
{
    public Material OutPrime;
    public Material OutSecondary;
    public int XMax;
    public int YMax;
    public int XCurve;
    public int YCurve;
    public int AmpCurve;
    public int DevCurve;
};


public class Machine : MonoBehaviour
{
    [SerializeField] private Port PortIn1;
    [SerializeField] private Port PortIn2;
    [SerializeField] private Port PortOut1;
    [SerializeField] private Port PortOut2;

    private PipeInputHandler myFirstPipe;
    private PipeInputHandler mySecondPipe;

    bool isSet = false;

    const float MaxOutput = 50;

    struct Pair
    {
        public Material Prime;
        public Material Secondary;
    }
    static Dictionary<Pair, Combination> combinationAtlas;
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
        myFirstPipe.Handle(PortIn1.transform.position);
        mySecondPipe.Handle(PortIn2.transform.position);
    }

    public void Handle(Vector3 aTarget)
    {
        if (isSet)
        {
            print("ERROR: called Handle after Set");
            return;
        }

        aTarget.y = 0;
        transform.position = aTarget;

        myFirstPipe.Handle(PortIn1.transform.position);
        mySecondPipe.Handle(PortIn2.transform.position);
    }

    public void Set()
    {
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
        myCombidantion = combinationAtlas[key];

        PortOut1.material = myCombidantion.OutPrime;
        PortOut2.material = myCombidantion.OutSecondary;

        isSet = true;
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

        PrimeUsage *= Time.fixedDeltaTime;
        SecondaryUsage *= Time.fixedDeltaTime;

        float usage = myFirstPipe.port.Amount / PrimeUsage;
        if (mySecondPipe.port.Amount / SecondaryUsage < usage)
        {
            usage = mySecondPipe.port.Amount / SecondaryUsage;
        }
        if (usage > 1)
        {
            usage = 1;
        }

        PrimeUsage *= usage;
        SecondaryUsage *= usage;
        PortOut1.Amount += (Out * Efficiency) / 256 * MaxOutput * usage;
        PortOut2.Amount += ((16 - Out) * Efficiency) / 256 * MaxOutput * usage;
    }
}
