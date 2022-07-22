using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class TickableEffect
{
    protected Image _icon;
    protected string _name;
    protected string _description;
    protected float _duration;
    protected float _tickDelay;
    protected Effectable _effectable;

    public string Name { get => _name; }
    public string Description { get => _description; }
    public float Duration { get => _duration; }
    public float TickDelay { get => _tickDelay; }
    public Effectable Target { get => _effectable; set => _effectable = value; }
    public Image Icon { get => _icon; }

    protected delegate void EffectProcessDelegate();
    protected event EffectProcessDelegate EffectTickEvent;

    public TickableEffect(string name, string description, float duration, float tickDelay)
    {
        _name = name;
        _description = description;
        _duration = duration;
        _tickDelay = tickDelay;
    }

    public void ApplyDurationModifier(float mod)
    {
        if (CheckModifier(mod)) return;
        _duration *= mod;
    }

    public void DisapplyDurationModifier(float mod)
    {
        if (CheckModifier(mod)) return;
        _duration /= mod;
    }

    public void ApplyTickDelayModifier(float mod)
    {
        if (CheckModifier(mod)) return;
        _tickDelay *= mod;
    }

    public void DisapplyTickDelayModifier(float mod)
    {
        if (CheckModifier(mod)) return;
        _tickDelay /= mod;
    }

    private bool CheckModifier(float mod)
    {
        if (mod <= 0)
        {
            Debug.LogWarning("Disallowed to use modifier <= 0");
            return false;
        }
        return true;
    }

    public IEnumerator ActivateEffectCoroutine()
    {
        OnEffectActivation();
        for (int i = 0; i < (int)_duration / _tickDelay; i++)
        {
            yield return new WaitForSeconds(_tickDelay);
            EffectTickEvent();
        }
        _effectable.RemoveEffect(this);
    }

    protected abstract void OnEffectActivation();
}
