using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Layers : MonoBehaviourPunCallbacks
{
    public static readonly int masterLayer = 1 << 6 | 1 << 12 | 1 << 17;
    public static readonly int masterUnit = 1 << 6;
    public static readonly int masterBuilding = 1 << 12;
    public static readonly int masterHero = 1 << 17;

    public static readonly int userLayer = 1 << 7 | 1 << 13 | 1 << 18;
    public static readonly int userUnit = 1 << 7;
    public static readonly int userBuilding = 1 << 13;
    public static readonly int userHero = 1 << 18;

    public static readonly int HeroLayer = 1 << 17 | 1 << 18;
}
