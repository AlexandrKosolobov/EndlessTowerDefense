using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ResistTable is an shell for a Dictionary<string, float>. <br/>
/// To add resist modificator use ApplyResist(string type, float mod) //!mod > 0!// <br/>
/// To remove resist modificator use DisapplyResist(string type, float mod) //!mod > 0!// <br/>
/// To calculate final damage use ModifyDamageByResist(...) <br/>
/// P.S. Use <see cref="ApplyResist"/> and <see cref="DisapplyResist"/> with the same parameters <br/>
/// </summary>
public class ResistTable
{
    private static float DEFAULT_RESIST_COEFFICIENT = 1f;
    private Dictionary<string, float> ResistDictionary = new Dictionary<string, float>();

    public static float ConvertResistCoefficientToPercentage(float coefficient)
    {
        return 100 - 100 * coefficient ;
    }

    public static float ConvertResistPercantageToCoefficient(float percentage)
    {
        return (100 - percentage) / 100;
    }

    public float ModifyDamageByResists(Damage damage)
    {
        if (ResistDictionary.TryGetValue(damage.Type, out float resist))
        {
            return damage.Amount * resist;
        }
        else
        {
            ResistDictionary.Add(damage.Type, DEFAULT_RESIST_COEFFICIENT);
            return damage.Amount;
        }
    }

    public float ModifyDamageByResists(string type, float damage)
    {
        if (ResistDictionary.TryGetValue(type, out float resist))
        {
            return damage * resist;
        }
        else
        {
            ResistDictionary.Add(type, DEFAULT_RESIST_COEFFICIENT);
            return damage;
        }
    }

    /// <summary>
    /// Disapplies resistance modificator to the owner of ResistTable. <br/>
    /// Use the same resistance modificator for applying and disapplying. <br/>
    /// <see cref="ApplyResist"/>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="mod">Resistance modificator must be > 0 </param>
    public void DisapplyResist(string type, float mod)
    {
        if (mod <= 0)
        {
            Debug.Log("Unable to disapply resist modificator <= 0");
            return;
        }

        if (ResistDictionary.TryGetValue(type, out float oldResist))
        {
            ResistDictionary[type] =  oldResist / mod;
        }
        else
        {
            ResistDictionary.Add(type, 1 / mod);
        }
    }

    /// <summary>
    /// Applies resistance modidficator to the owner of ResistTable. <br/>
    /// Use the same resistance modificator for applying and disapplying. <br/>
    /// <see cref="DisapplyResist"/>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="mod">Resistance modificator must be > 0 </param>
    public void ApplyResist(string type, float mod)
    {
        if (mod <= 0)
        {
            Debug.Log("Unable to apply resist modificator <= 0");
            return;
        }

        if (ResistDictionary.TryGetValue(type, out float oldResist))
        {
            ResistDictionary[type] =  oldResist * mod;
        }
        else
        {
            ResistDictionary.Add(type, mod);
        }
    }

    public float GetResistPercentage(string type)
    {
        if (ResistDictionary.TryGetValue(type, out float resist))
        {
            return ConvertResistCoefficientToPercentage(resist);
        }
        else
        {
            ResistDictionary.Add(type, DEFAULT_RESIST_COEFFICIENT);
            return ConvertResistCoefficientToPercentage(DEFAULT_RESIST_COEFFICIENT);
        }
    }

    public float GetResistCoefficient(string type)
    {
        if (ResistDictionary.TryGetValue(type, out float resist))
        {
            return resist;
        }
        else
        {
            ResistDictionary.Add(type, DEFAULT_RESIST_COEFFICIENT);
            return DEFAULT_RESIST_COEFFICIENT;
        }
    }

    public Dictionary<string, float> GetResistTableCopy()
    {
        return new Dictionary<string, float>(ResistDictionary);
    }
}