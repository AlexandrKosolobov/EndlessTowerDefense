using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DOTEffect : TickableEffect
{
    private Damagable _damagable;
    public Damage _damage;

    public DOTEffect(string name, string description, float duration, float tickDelay, Damage damage)
    : base(name, description, duration, tickDelay)
    {
        _damage = damage;
    }

    private void DealDamage()
    {
        if (_damagable != null)
        {
        _damagable.ApplyDamage(_damage);
        }
    }

    protected override void OnEffectActivation()
    {
        if (Target.TryGetComponent<Damagable>(out _damagable))
        {
            EffectTickEvent += DealDamage;
        }
    }
}
