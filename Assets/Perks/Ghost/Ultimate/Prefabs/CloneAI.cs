using UnityEngine;

public class CloneAI : CreatureAI
{
    [Header("VFX ARRAY")]
    [SerializeField] private VFX[] _initialVFXArray;
    [SerializeField] private VFX[] _constantVFXArray;
    private VFXManager _VFXManager;

    public override void Init(LayerMask creatureLayer, LayerMask targetLayer, CreatureData creatureData)
    {
        base.Init(creatureLayer, targetLayer, creatureData);

        _VFXManager = VFXManager.Instance;

        for (int i = 0; i < _initialVFXArray.Length; i++)
        {
            _VFXManager.AddVFX(_initialVFXArray[i], true, 3f, _Transform.position, _Transform.rotation, _Transform);
        }

        for (int i = 0; i < _constantVFXArray.Length; i++)
        {
            _VFXManager.AddVFX(_constantVFXArray[i], _Transform);
        }
    }

    public override void Cleanup()
    {
        for (int i = 0; i < _initialVFXArray.Length; i++)
        {
            _VFXManager.RemoveVFX(_initialVFXArray[i]);
        }

        for (int i = 0; i < _constantVFXArray.Length; i++)
        {
            _VFXManager.RemoveVFX(_constantVFXArray[i]);
        }

        base.Cleanup();
    }
}
