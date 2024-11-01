using UnityEngine;

[CreateAssetMenu(fileName = "Combo Data", menuName = "Scriptable Object/Data/Combo Data/Bloodwave")]
public class BloodwaveCombo : ComboData
{
    [Header("COMBO FINISHER")]
    [SerializeField] private VFX _bloodwaveVFX;

    protected override void PerformComboFinisher(AttackType attackType)
    {
        _VFXManager.AddStaticVFX(_bloodwaveVFX, _Transform.position, _Transform.rotation, 3f);
    }
}