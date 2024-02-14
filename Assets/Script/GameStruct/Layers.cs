using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Layers : MonoBehaviourPunCallbacks
{
    public const int masterLayer = 1 << 6 | 1 << 12 | 1 << 17;
    public const int masterUnit = 1 << 6;
    public const int masterBuilding = 1 << 12;
    public const int masterHero = 1 << 17;

    public const int userLayer = 1 << 7 | 1 << 13 | 1 << 18;
    public const int userUnit = 1 << 7;
    public const int userBuilding = 1 << 13;
    public const int userHero = 1 << 18;

    public static readonly int HeroLayer = 1 << 17 | 1 << 18;
}
