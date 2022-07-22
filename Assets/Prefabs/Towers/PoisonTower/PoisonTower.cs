using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : TowerBehaviour
{
    private void Awake()
    {
        _attackEffect = new DOTEffect("Poison", "Deals damage over time", 10, 0.1f, new Damage(1, "Poison"));
        _attackRate = 1f;
        _critChance = 0.1f;
        _critDamage = 1.5f;
        _damage = new Damage(0, "Physical");
        _projectileSpeed = 2;
        _range = 0.7f;
        _rotationSpeed = 90;
    }
}
