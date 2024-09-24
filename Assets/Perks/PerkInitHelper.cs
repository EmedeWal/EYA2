using UnityEngine;

public class PerkInitHelper : MonoBehaviour
{
    private void Awake()
    {
        StancePerk[] stancePerks = GetComponentsInChildren<StancePerk>();
        foreach (var stancePerk in stancePerks)
        {
            stancePerk.Init(); Debug.Log("Init");
        }
    }
}
