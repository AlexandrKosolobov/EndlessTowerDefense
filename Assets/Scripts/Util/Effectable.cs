using System.Collections.Generic;
using UnityEngine;

public class Effectable : MonoBehaviour
{
    protected List<TickableEffect> Effects = new List<TickableEffect>();

    public void ApplyEffect(TickableEffect effect)
    {
        effect.Target = this;
        Effects.Add(effect);
        StartCoroutine(effect.ActivateEffectCoroutine());
    }

    public void RemoveEffect(TickableEffect effect)
    {
        StopCoroutine(effect.ActivateEffectCoroutine());
        Effects.Remove(effect);
    }
}
