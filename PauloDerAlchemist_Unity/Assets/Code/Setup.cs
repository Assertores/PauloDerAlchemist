using System;
using System.Collections;
using System.Collections.Generic;

public enum Material
{
    Fire = 0, Water = 1, Earth = 2, Air = 3,
    Helium = 4, Ozone = 5, Carbon = 6,
    Methanol = 7, Propane = 8, Ethane = 9, Acetylene = 10,
    PVC = 11, PTFE = 12, SiO2 = 13,
    Plastic = 14, Glass = 15,
    Steel = 16, Aluminum = 17, Brass = 18,
    Plutonium = 19, Radon = 20, Uranium = 21, Fermium = 22,
    Silicon = 23, Tungsten = 24, Silver = 25,
    Mercury = 26, Platinum = 27,
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
        if (aMaterial <= Material.Air)
        {
            return 0;
        }
        if (aMaterial <= Material.Carbon)
        {
            return 1;
        }
        if (aMaterial <= Material.Acetylene)
        {
            return 2;
        }
        if (aMaterial <= Material.SiO2)
        {
            return 3;
        }
        if (aMaterial <= Material.Glass)
        {
            return 4;
        }
        if (aMaterial <= Material.Brass)
        {
            return 5;
        }
        if (aMaterial <= Material.Fermium)
        {
            return 6;
        }
        if (aMaterial <= Material.Silver)
        {
            return 7;
        }
        if (aMaterial <= Material.Platinum)
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
                list.Add(Material.Fire);
                list.Add(Material.Water);
                list.Add(Material.Earth);
                list.Add(Material.Air);
                break;
            case 1:
                list.Add(Material.Helium);
                list.Add(Material.Ozone);
                list.Add(Material.Carbon);
                break;
            case 2:
                list.Add(Material.Methanol);
                list.Add(Material.Propane);
                list.Add(Material.Ethane);
                list.Add(Material.Acetylene);
                break;
            case 3:
                list.Add(Material.PVC);
                list.Add(Material.PTFE);
                list.Add(Material.SiO2);
                break;
            case 4:
                list.Add(Material.Plastic);
                list.Add(Material.Glass);
                break;
            case 5:
                list.Add(Material.Steel);
                list.Add(Material.Aluminum);
                list.Add(Material.Brass);
                break;
            case 6:
                list.Add(Material.Plutonium);
                list.Add(Material.Radon);
                list.Add(Material.Uranium);
                list.Add(Material.Fermium);
                break;
            case 7:
                list.Add(Material.Silicon);
                list.Add(Material.Tungsten);
                list.Add(Material.Silver);
                break;
            case 8:
                list.Add(Material.Mercury);
                list.Add(Material.Platinum);
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

        var notUsedGrade = new List<List<Material>>();
        for (int i = 0; i < 10; i++)
        {
            notUsedGrade.Add(Extention.GetMaterialOfGrade(i));
            Extention.Shuffle(notUsedGrade[i], random);
        }

        var combinations = new List<Pair>();

        for (Material lhs = 0; lhs < Material.Gold; lhs++)
        {
            for (Material rhs = lhs; rhs < Material.Gold; rhs++)
            {
                combinations.Add(new Pair(lhs, rhs));
            }
        }
        Extention.Shuffle(combinations, random);

        foreach (var it in combinations)
        {
            var gread = Extention.GetGrade(it.Secondary);
            var secondary = new List<Material>();
            for (int i = 0; i <= gread; i++)
            {
                secondary.AddRange(Extention.GetMaterialOfGrade(i));
            }
            Combination combination;
            if (notUsedGrade[gread + 1].Count > 0)
            {
                combination.OutPrime = notUsedGrade[gread + 1][0];
                notUsedGrade[gread + 1].RemoveAt(0);
            }
            else
            {
                var primary = Extention.GetMaterialOfGrade(gread);
                combination.OutPrime = primary[random.Next(primary.Count)];
            }
            combination.OutSecondary = secondary[random.Next(secondary.Count)];
            combination.XMax = random.Next(16);
            combination.YMax = random.Next(16);
            combination.XCurve = random.Next(16);
            combination.YCurve = random.Next(16);
            combination.DevCurve = random.Next(16);
            combination.AmpCurve = random.Next(16) - 8;
            if (combination.AmpCurve + combination.YCurve < 0)
            {
                combination.AmpCurve = -combination.YCurve;
            }
            else if (combination.AmpCurve + combination.YCurve > 16)
            {
                combination.AmpCurve = 16 - combination.YCurve;
            }
            Machine.combinationAtlas.Add(it, combination);
        }

        // TODO: deal with Gold
    }
}
