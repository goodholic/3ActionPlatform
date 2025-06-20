using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuffSO", menuName = "Scriptable Objects/Datas/BuffSO", order = 0)]
public class StatusEffectSO : ScriptableObject
{
    public int ID;
    public List<StatusEffectData> StatusEffects;
}