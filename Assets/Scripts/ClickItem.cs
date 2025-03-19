using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New item",menuName="Idle Game/Item")]
public class ClickItem : ScriptableObject
{
    public string ItemName;
    public float BasePrice;
    private const float Multiplier = 1.07f; //1.07-1.15
    public float BaseIncome;

    public Sprite ItemImage; 
    public Sprite UnknownItemImage;

    public float CalculateCost(int amount)
    {
        float newPrice = BasePrice * MathF.Pow(Multiplier, amount);
        float roundedPrice = (float)Mathf.Round(newPrice*100) / 100;
        return roundedPrice;
    }

    public float CalculateIncome(int amount)
    {
        return BaseIncome * amount;
    }
}