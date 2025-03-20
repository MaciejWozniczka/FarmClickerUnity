using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Items
    [System.Serializable]
    public class AnyItem
    {
        [HideInInspector] public int ItemAmount;
        public ClickItem Item;
        public bool Unlocked;
        [HideInInspector] public bool Created;
        [HideInInspector] public ItemHolder Holder;
    }
    [Header("ITEMS")]
    public List<AnyItem> ItemList = new List<AnyItem>();

    // Money
    [Header("MONEY")]
    public float Money;
    public Text TotalMoneyText;
    public Text TotalIncomeText;

    // User Interface
    [Header("USER INTERFACE")]
    public GameObject ItemHolder;
    public Transform Grid;
}