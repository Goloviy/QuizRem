using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareResourcesManager : MonoBehaviourSingleton<ShareResourcesManager>
{
    [SerializeField] private Sprite missedIconDefault;
    
    [Header("")] public List<KnowledgeCategoryIcon> categoryIcons;
    [Header("")] public List<CurrencyIcon> currencyIcons;
    
    protected override void SingletonAwakened()
    {
        base.SingletonAwakened();
    }

    public Sprite GetCurrencyIcon(CurrencyType _currency)
    {
        for (int i = 0; i < currencyIcons.Count; i++)
        {
            if (currencyIcons[i].currency == _currency)
            {
                return currencyIcons[i].icon;
            }
        }

        return missedIconDefault;
    }
}

[Serializable]
public struct KnowledgeCategoryIcon
{
    public KnowledgeCategory category;
    public Sprite icon;
}

[Serializable]
public struct CurrencyIcon
{
    public CurrencyType currency;
    public Sprite icon;
}