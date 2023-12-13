using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOSkill : ScriptableObject
{
    public int skillLevel;
    public int reqLevel;
    public float damage;
    public int reqMp;
    public string animationName;
    public float coolTime;
    public Sprite icon;
    public GameObject[] skillEffect;
}
