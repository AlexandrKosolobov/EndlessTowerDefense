using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Damagable : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private Image _healthBarFilling;
    [SerializeField] private Canvas _healthBarCanvas;
    [SerializeField] private Gradient _gradient;
    private Camera _camera;

    public float Health { get => _health; private set => _health = value; }
    public float MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public ResistTable ResistTable { get; private set; }

    private void Awake()
    {
        ResistTable = new ResistTable();
        _camera = Camera.main;
        ModifyHealthBar();
    }

    private void ModifyHealthBar()
    {
        float healthPercentage = _health / _maxHealth;
        _healthBarFilling.fillAmount = healthPercentage;
        _healthBarFilling.color = _gradient.Evaluate(healthPercentage);
    }

    public void ApplyDamage(Damage damage)
    {
        _health -= ResistTable.ModifyDamageByResists(damage);
        if (_health <= 0)
        {
            Die();
        }
        ModifyHealthBar();
    }

    public void ApplyDamage(string type, float damage)
    {
        Health -= ResistTable.ModifyDamageByResists(type, damage);
        if (Health <= 0)
        {
            Die();
        }
        ModifyHealthBar();
    }

    public void ApplyHeal(float heal)
    {
        Health += heal;
        if (Health > MaxHealth)
        {
            Overheal();
        }
        ModifyHealthBar();
    }

    private void LateUpdate()
    {
        _healthBarCanvas.transform.LookAt(new Vector3(_healthBarCanvas.transform.position.x, _camera.transform.position.y, _camera.transform.position.z + 1));
        _healthBarCanvas.transform.Rotate(0, 180, 0);
    }

    protected virtual void Overheal()
    {
        Health = MaxHealth;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}