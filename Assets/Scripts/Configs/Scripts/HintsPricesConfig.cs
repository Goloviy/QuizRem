using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Hints price", menuName ="Gameplay/Hints price")]
public class HintsPricesConfig : BaseConfig
{
    public List<HintPricesData> data;
}
[Serializable]
public class HintPricesData
{
    public HintsType hintType;
    public int softCurrencyPrice;
    public int hardCurrencyPrice;
    public int sessionPrice;
}
