using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController
{
    private Dictionary<HintsType, Hint> hints = new Dictionary<HintsType, Hint>(3);
    public Action<Hint> OnHintWasUpdated;

    public InventoryController(List<Hint> _hints)
    {
        for (var i = 0; i < _hints.Count; i++)
        {
            hints.Add(_hints[i].hintType, _hints[i]);
        }
    }

    public int GetHintCount(HintsType _hint)
    {
        return hints.TryGetValue(_hint, out var hint) ? hint.count : 0;
    }

    public void ChangeHintsCount(HintsType _hint, int _count)
    {
        if (hints.ContainsKey(_hint))
        {
            hints[_hint].count = _count;
            OnHintWasUpdated?.Invoke(hints[_hint]);
        }
        else
        {
            Debug.LogError("Hint not found to InventoryController");
        }
    }

    public void AddHint(HintsType _hint, int _count)
    {
        if (_count < 0)
        {
            Debug.LogError("Hint count cannot be negative");
            return;
        }
        
        hints[_hint].count += _count;
    }

    public void HintWasUsed(HintsType _type)
    {
        hints[_type].count--;
    }
}

public class Hint
{
    public HintsType hintType;
    public int count;
    public int sessionPrice;
}