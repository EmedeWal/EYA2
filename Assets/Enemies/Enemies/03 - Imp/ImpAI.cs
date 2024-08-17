using UnityEngine;

public class ImpAI : RangedEnemyAI
{
    [Header("FIRE DATA")]
    [SerializeField] private EnemyRangedData _fireorbData;

    [Header("DEATH EXPLOSION")]
    [SerializeField] private GameObject explosionPrefab;

    private void Awake()
    {
        DetermineFireData();
        SetReferences();
    }

    public override void DetermineFireData()
    {
        SetFireData(_fireorbData);
    }

    private void OnDestroy()
    {
        //if (GetComponent<EnemyHealth>().AtMinValue())
        //{
        //    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        //}
    }
}