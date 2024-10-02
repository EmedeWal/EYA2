using UnityEngine;

public class CloneAI : CreatureAI
{
    [Header("VFX ARRAY")]
    [SerializeField] private VFX[] _initialVFXArray;
    [SerializeField] private VFX[] _constantVFXArray;
    private VFXManager _VFXManager;

    public override void Init(LayerMask targetLayer)
    {
        base.Init(targetLayer);

        _VFXManager = VFXManager.Instance;

        for (int i = 0; i < _initialVFXArray.Length; i++)
        {
            _VFXManager.AddVFX(_initialVFXArray[i], _Transform, true, 3f);
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
