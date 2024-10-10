using UnityEngine;

[CreateAssetMenu(fileName = "Combo Data", menuName = "Scriptable Object/Data/Combo Data/Bloodwave")]
public class BloodwaveCombo : ComboData
{
    [Header("COMBO FINISHER")]
    [SerializeField] private VFX _bloodwaveVFX;

    protected override void PerformComboFinisher(AttackType attackType)
    {
        _VFXManager.AddVFX(_bloodwaveVFX, true, 3f, _Transform.position, _Transform.rotation);
    }
}