using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TowerBehaviour : MonoBehaviour
{
    private static float SEARCH_DELAY = 0.2f;
    private static float MIN_QUATERNION_OFFSET = 0.1f;
    private static float DEFAULT_WAIT_TIME_SECONDS = 0.1f;
    [SerializeField] protected Damage _damage;
    [SerializeField] protected TickableEffect _attackEffect;
    [SerializeField] protected float _range;
    [SerializeField] protected float _rotationSpeed;
    [SerializeField] protected float _attackRate;
    [SerializeField] protected float _projectileSpeed;
    [SerializeField] protected float _critChance;
    [SerializeField] protected float _critDamage;
    [SerializeField] protected Transform _shootPosition;
    [SerializeField] protected Projectile _projectile;
    [SerializeField] private Damagable _target;

    private bool TargetIsAliveAndNear => _target != null && Vector3.Distance(transform.position, _target.transform.position) <= _range;
    private bool IsOnCooldown = false;
    private bool SearchingStarted = false;
    private bool LockedOnTarget => Math.Abs(transform.rotation.z - _deltaQuaternion.z) < MIN_QUATERNION_OFFSET;

    private void Update()
    {
        if (TargetIsAliveAndNear)
        {
            Rotate();
            if (LockedOnTarget && !IsOnCooldown)
            {
                Attack();
                
            }
        }
        else if (!SearchingStarted)
        {
            StartCoroutine(StartSearchNewTarget());
        }
    }

    public void ApplyDamageModifier(float mod)
    {
        if (mod <= 0)
        {
            Debug.LogWarning("Disallowed to use modifier <= 0");
            return;
        }
        _damage.Amount *= mod;
    }

    public void DisapplyDamageModifier(float mod)
    {
        if (mod <= 0)
        {
            Debug.LogWarning("Disallowed to use modifier <= 0");
            return;
        }
        _damage.Amount /= mod;
    }

    private Quaternion _deltaQuaternion;
    private void Rotate()
    {
        _deltaQuaternion = Quaternion.LookRotation(_target.transform.position, transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _deltaQuaternion, Time.deltaTime * _rotationSpeed);
    }

    private void Attack()
    {
        Projectile projectile = Instantiate<Projectile>(_projectile, _shootPosition);
        projectile.SetDamage(_damage);
        projectile.SetProjectileSpeed(_projectileSpeed);
        projectile.SetProjectileEffect(_attackEffect);
        projectile.SetTarget(_target.transform);
        StartCoroutine(WaitCooldown());
    }

    private IEnumerator StartSearchNewTarget()
    {
        SearchingStarted = true;
        List<Damagable> targetsDamagable;

        do
        {
            yield return new WaitForSeconds(SEARCH_DELAY);
            FindAllDamagableInRange(out targetsDamagable);
        }
        while (targetsDamagable.Count == 0);

        _target = ChooseClosestDamagableInRange(targetsDamagable);
        SearchingStarted = false;
    }

    private void FindAllDamagableInRange(out List<Damagable> targetsDamagable)
    {
        targetsDamagable = new List<Damagable>();
        Collider2D[] targetsColliders = Physics2D.OverlapCircleAll(transform.position, _range);
        foreach (Collider2D targetCollider in targetsColliders)
        {
            if (targetCollider.TryGetComponent<Damagable>(out Damagable damagable))
            {
                targetsDamagable.Add(damagable);
            }
        }
    }

    private IEnumerator WaitCooldown()
    {
        IsOnCooldown = true;
        yield return new WaitForSeconds(1 / _attackRate);
        IsOnCooldown = false;
    }

    protected virtual Damagable ChooseClosestDamagableInRange(List<Damagable> targetsDamagable)
    {
        Damagable resultTarget = targetsDamagable[0];
        foreach (Damagable anyTarget in targetsDamagable)
        {
            if (anyTarget == null) continue;
            resultTarget = Vector3.Distance(anyTarget.transform.position, transform.position)
            < Vector3.Distance(resultTarget.transform.position, transform.position) ? anyTarget : resultTarget;
        }
        return resultTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
