using System;
using System.Collections;
using System.Collections.Generic;

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

public struct Pair
{
    public Pair(Material aPrime, Material aSecondary)
    {
        Prime = aPrime;
        Secondary = aSecondary;
    }
    public Material Prime;
    public Material Secondary;
}

static class Extention
{
    public static void Shuffle<T>(this IList<T> list, Random rng)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static int GetGrade(Material aMaterial)
    {
        if(aMaterial <= Material.Luft)
        {
            return 0;
        }
        if(aMaterial <= Material.Kohlenstoff)
        {
            return 1;
        }
        if(aMaterial <= Material.Acetaldehyd)
        {
            return 2;
        }
        if(aMaterial <= Material.Siliciumdioxid)
        {
            return 3;
        }
        if(aMaterial <= Material.Glas)
        {
            return 4;
        }
        if(aMaterial <= Material.Messing)
        {
            return 5;
        }
        if(aMaterial <= Material.Fermium)
        {
            return 6;
        }
        if(aMaterial <= Material.Silber)
        {
            return 7;
        }
        if(aMaterial <= Material.Platin)
        {
            return 8;
        }
        return 9;
    }

    public static List<Material> GetMaterialOfGrade(int aGrade)
    {
        var list = new List<Material>();
        switch (aGrade)
        {
            case 0:
                list.Add(Material.Feuer);
                list.Add(Material.Wasser);
                list.Add(Material.Erde);
                list.Add(Material.Luft);
                break;
            case 1:
                list.Add(Material.Helium);
                list.Add(Material.Ozon);
                list.Add(Material.Kohlenstoff);
                break;
            case 2:
                list.Add(Material.Methanol);
                list.Add(Material.Propan);
                list.Add(Material.Ethan);
                list.Add(Material.Acetaldehyd);
                break;
            case 3:
                list.Add(Material.Polyvinylchlorid);
                list.Add(Material.Polytetrafluorethylen);
                list.Add(Material.Siliciumdioxid);
                break;
            case 4:
                list.Add(Material.Plastik);
                list.Add(Material.Glas);
                break;
            case 5:
                list.Add(Material.Stahl);
                list.Add(Material.Aluminium);
                list.Add(Material.Messing);
                break;
            case 6:
                list.Add(Material.Plutonium);
                list.Add(Material.Radon);
                list.Add(Material.Uran);
                list.Add(Material.Fermium);
                break;
            case 7:
                list.Add(Material.Silicium);
                list.Add(Material.Wolfram);
                list.Add(Material.Silber);
                break;
            case 8:
                list.Add(Material.Quecksilber);
                list.Add(Material.Platin);
                break;
            case 9:
                list.Add(Material.Gold);
                break;
        }
        return list;
    }
}

public class Setup : UnityEngine.MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Machine.combinationAtlas = new Dictionary<Pair, Combination>();
        var random = new Random();

        for (Material lhs = 0; lhs < Material.Gold; lhs++)
        {
            for (Material rhs = lhs; rhs < Material.Gold; rhs++)
            {
                var gread = Extention.GetGrade(rhs);
                var primary = Extention.GetMaterialOfGrade(gread + 1);
                //primary.AddRange(Extention.GetMaterialOfGrade(gread));
                var secondary = new List<Material>();
                for(int i = 0; i <= gread; i++)
                {
                    secondary.AddRange(Extention.GetMaterialOfGrade(i));
                }
                Combination combination;
                combination.OutPrime = primary[random.Next(primary.Count)];
                combination.OutSecondary = secondary[random.Next(secondary.Count)];
                combination.XMax = random.Next(16);
                combination.YMax = random.Next(16);
                combination.XCurve = random.Next(16);
                combination.YCurve = random.Next(16);
                combination.DevCurve = random.Next(16);
                combination.AmpCurve = random.Next(16) - 8;
                if(combination.AmpCurve + combination.YCurve < 0)
                {
                    combination.AmpCurve = -combination.YCurve;
                }
                else if(combination.AmpCurve + combination.YCurve > 16)
                {
                    combination.AmpCurve = 16 - combination.YCurve;
                }
                Machine.combinationAtlas.Add(new Pair(lhs, rhs), combination);
            }

            // TODO: deal with Gold
        }

    }
}
