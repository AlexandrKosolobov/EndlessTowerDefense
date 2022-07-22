using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private static Converter<ContactPoint2D, Transform> PojectileConverter
        = new Converter<ContactPoint2D, Transform>(ContactPointToTransform);
    [SerializeField] private Damage _damage;
    [SerializeField] private TickableEffect _effect;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private Transform _target;

    public bool TargetDied => _target == null;

    private static Transform ContactPointToTransform(ContactPoint2D point)
    {
        return point.collider.transform;
    }

    private void Update()
    {
        MoveToTarget();
    }

    public void SetDamage(Damage damage)
    {
        _damage = damage;
    }

    public void SetProjectileEffect(TickableEffect effect)
    {
        _effect = effect;
    }

    public void SetProjectileSpeed(float projectileSpeed)
    {
        _projectileSpeed = projectileSpeed;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void MoveToTarget()
    {
        if (TargetDied)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _projectileSpeed);
        if (Vector3.Distance(transform.position, _target.position) < 0.05f)
        {
            TryApplyDamage();
            TryApplyEffect();
            Destroy(gameObject);
        }
    }

    private void TryApplyDamage()
    {
        if (_target.TryGetComponent<Damagable>(out Damagable damagable))
        {
            damagable.ApplyDamage(_damage);
        }
    }

    private void TryApplyEffect()
    {
        if (_target.TryGetComponent<Effectable>(out Effectable effectable))
        {
            effectable.ApplyEffect(_effect);
        }
    }
}
