using System;
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

    public int CalculateCost(int amount)
    {
        float newPrice = BasePrice * MathF.Pow(Multiplier, amount);
        int roundedPrice = (int)Mathf.Round(newPrice);
        return roundedPrice;
    }

    public int CalculateIncome(int amount)
    {
        return (int)Mathf.Round(BaseIncome * amount);
    }
}