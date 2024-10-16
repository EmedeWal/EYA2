using System.Collections.Generic;
using UnityEngine;

public class VFXManager : SingletonBase
{
    #region Singleton
    public static VFXManager Instance;
    public override void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _transform = transform;
    }
    #endregion

    private Transform _transform;

    private List<VFX> _activeVFXList = new();

    public VFX AddStaticVFX(VFX vfxPrefab, Vector3 position, Quaternion rotation, float destroyDelay)
    {
        VFX vfxInstance = Instantiate(vfxPrefab, position, rotation);

        vfxInstance.transform.SetParent(_transform, true);
        vfxInstance.Init(vfxInstance.transform);
        _activeVFXList.Add(vfxInstance);

        StartCoroutine(DestroyVFXAfterDelay(vfxInstance, destroyDelay));

        return vfxInstance; 
    }

    public VFX AddStaticVFX(VFX vfxPrefab, Vector3 position, Quaternion rotation)
    {
        VFX vfxInstance = Instantiate(vfxPrefab, position, rotation);

        vfxInstance.transform.SetParent(_transform, true);
        vfxInstance.Init(vfxInstance.transform);
        _activeVFXList.Add(vfxInstance);

        return vfxInstance;
    }

    public VFX AddMovingVFX(VFX vfxPrefab, Transform followTarget, float destroyDelay)
    {
        VFX vfxInstance = Instantiate(vfxPrefab, followTarget.position, followTarget.rotation);

        vfxInstance.transform.SetParent(_transform, true);
        vfxInstance.Init(followTarget);
        _activeVFXList.Add(vfxInstance);

        StartCoroutine(DestroyVFXAfterDelay(vfxInstance, destroyDelay));

        return vfxInstance;
    }

    public VFX AddMovingVFX(VFX vfxPrefab, Transform followTarget)
    {
        VFX vfxInstance = Instantiate(vfxPrefab, followTarget.position, followTarget.rotation);

        vfxInstance.transform.SetParent(_transform, true);
        vfxInstance.Init(followTarget);
        _activeVFXList.Add(vfxInstance);

        return vfxInstance;
    }

    public void Tick(float delta)
    {
        for (int i = 0; i < _activeVFXList.Count; i++)
        {
            _activeVFXList[i].Tick();
        }
    }

    public void RemoveVFX(VFX vfx, float destroyDelay = 0f)
    {
        if (_activeVFXList.Contains(vfx))
        {
            vfx.Deactivate();
            StartCoroutine(DestroyVFXAfterDelay(vfx, destroyDelay));
        }
    }

    private IEnumerator<WaitForSeconds> DestroyVFXAfterDelay(VFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_activeVFXList.Contains(vfx))
        {
            _activeVFXList.Remove(vfx);
        }

        Destroy(vfx.gameObject);
    }
}