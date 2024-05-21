using System.Collections;
using UnityEngine;

public class OrcAI : MeleeEnemyAI
{
    [Header("ATTACK DATA")]
    [SerializeField] private EnemyMeleeData _singeSlashData;
    [SerializeField] private EnemyMeleeData _doubleSlashData;
    [SerializeField] private EnemyMeleeData _spinSlashData;
    private int _attackNumber;

    [Header("SPIN ATTACK")]
    [SerializeField] private float _spinRadius = 3f;

    private void Awake()
    {
        DetermineAttackData();
        SetReferences();
    }

    protected override void DetermineAttackData()
    {
        _attackNumber = Random.Range(0, 3);

        if (_attackNumber == 0)
        {
            SetAttackData(_singeSlashData);
        }
        else if (_attackNumber == 1)
        {
            SetAttackData(_doubleSlashData);
        }
        else
        {
            SetAttackData(_spinSlashData);
        }
    }

    protected override void ChargeStart()
    {
        if (_attackNumber != 2)
        {
            PlayMeleeAnimation();
        }
    }

    protected override void AttackAction()
    {
        if (_attackNumber == 0)
        {
            SingleSlash();
        }
        else if (_attackNumber == 1)
        {
            DoubleSlash();
        }
        else
        {
            SpinSlash();
            PlayMeleeAnimation();
        }
    }

    private void SingleSlash()
    {
        DealDamage(CastBox());
    }

    private void DoubleSlash()
    {
        StartCoroutine(DoubleSlashCoroutine());
    }

    private IEnumerator DoubleSlashCoroutine()
    {
        for (int i = 0; i < 2; i++)
        {
            DealDamage(CastBox());

            yield return new WaitForSeconds(0.35f);
        }
    }

    private void SpinSlash()
    {
        DealDamage(CastSphere());
    }

    private void PlayMeleeAnimation()
    {
        PlayAnimation(MeleeData.AnimationParameter);
    }

    private Collider[] CastSphere()
    {
        return Physics.OverlapSphere(transform.position + new Vector3(0, 1f, 0), _spinRadius, PlayerLayer);
    }
}