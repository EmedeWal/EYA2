using UnityEngine;

[CreateAssetMenu(fileName = "Bloodwave Stats", menuName = "Scriptable Object/Stats/Bloodwave Stats")]
public class BloodwaveStats : ScriptableObject
{
    [HideInInspector] public float Damage;
    [HideInInspector] public bool MultiplierEnabled;

    public void Init()
    {
        Damage = 50;
        MultiplierEnabled = false;
    }

    public void ActivateMultiplier()
    {
        MultiplierEnabled = true;
    }
}