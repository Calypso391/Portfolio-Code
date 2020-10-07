using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// Bit mask to represent an assortment of colors
/// </summary>
[Flags]
[Serializable]
public enum EColorFlag : int
{
    None = 0x0,
    White = 0x1,
    Red = 0x2,
    Blue = 0x4,
    Yellow = 0x8,
    Green = 0x10,
    Orange = 0x20,
    Pink = 0x40,
    Cyan = 0x80,
    Purple = 0x100,
    NavyBlue = 0x200,
    Brown = 0x400,
    ForestGreen = 0x800,
}

public class EColorRGBHelper
{
    /// <summary>
    /// Given a set of flags, return a random single flag from within it.
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static EColorFlag GetRandomFlagFromFlags(EColorFlag flags)
    {
        // straight up copied from https://stackoverflow.com/a/3002313

        if (flags == 0) {
            return EColorFlag.None;
        }

        var matching = Enum.GetValues(typeof(EColorFlag))
                    .Cast<EColorFlag>()
                    .Where(c => ((flags & c) == c && c != 0))
                    .ToArray();

        if (matching.Length == 0) {
            return EColorFlag.None;
        }

        EColorFlag myEnum = EColorFlag.None;
        bool foundColor = false;

        while (!foundColor) {
            myEnum = matching[new System.Random().Next(matching.Length)];
            if (myEnum == EColorFlag.None) {
                continue;
            }

            foundColor = true;
        }


        return myEnum;
    }

    /// <summary>
    /// Convert a single EColorFlag to an RGB Color (black if 0).
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static Color FromSingleFlagToRGB(EColorFlag flag)
    {
        Color output = Color.black;

        switch (flag) {
            case EColorFlag.White:
                output = Color.white;
                break;
            case EColorFlag.Red:
                output = Color.red;
                break;
            case EColorFlag.Blue:
                output = Color.blue;
                break;
            case EColorFlag.Yellow:
                output = Color.yellow;
                break;
            case EColorFlag.Green:
                output = Color.green;
                break;
            case EColorFlag.Orange:
                output = new Color(1.0f, 0.75f, 0.0f);
                break;
            case EColorFlag.Pink:
                output = new Color(1.0f, 0.75f, 0.80f);
                break;
            case EColorFlag.Cyan:
                output = Color.cyan;
                break;
            case EColorFlag.Purple:
                output = new Color(0.73f, 0.25f, 0.73f);
                break;
            case EColorFlag.NavyBlue:
                output = new Color(0.22f, 0.25f, 0.57f);
                break;
            case EColorFlag.Brown:
                output = new Color(.90f, 0.41f, 0.11f);
                break;
            case EColorFlag.ForestGreen:
                output = new Color(0.23f, 0.42f, 0.15f);
                break;
        }

        return output;
    }
}
