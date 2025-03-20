using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Items
    [System.Serializable]
    public class AnyItem
    {
        [HideInInspector] 
        public int ItemAmount;
        public ClickItem Item;
        public bool Unlocked;
        [HideInInspector]
        public bool Created;
        [HideInInspector]
        public ItemHolder Holder;
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

    void Awake()
    {
        instance = this;   
    }

    // Initialization
    void Start()
    {
        FillList();
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);

            foreach(var item in ItemList)
            {
                if (item.ItemAmount > 0)
                {
                    Money += item.Item.CalculateIncome(item.ItemAmount);
                    Money = (float)Mathf.Round(Money * 100) / 100;
                }
            }
        }
    }

    void FillList()
    {
        for (var i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].Unlocked)
            {
                if (ItemList[i].ItemAmount > 0 || ItemList[i].Created)
                {
                    // Skip adding duplicates
                    continue;
                }
                
                GameObject itemHolder = Instantiate(ItemHolder, Grid, false) as GameObject;
                ItemList[i].Holder = itemHolder.GetComponent<ItemHolder>();
                    
                if (ItemList[i].ItemAmount > 0)
                {
                    ItemList[i].Holder.itemImage.sprite = ItemList[i].Item.ItemImage;
                    ItemList[i].Holder.ItemNameText.text = ItemList[i].Item.ItemName;
                    ItemList[i].Holder.AmountText.text = "Ilość: " + ItemList[i].ItemAmount;
                    ItemList[i].Holder.IncomeText.text = "Dochód: " + ItemList[i].Item.CalculateIncome(ItemList[i].ItemAmount);
                    ItemList[i].Holder.CostText.text = "Koszt: " + ItemList[i].Item.CalculateCost(ItemList[i].ItemAmount);
                }
                else
                {
                    ItemList[i].Holder.itemImage.sprite = ItemList[i].Item.UnknownItemImage;
                    ItemList[i].Holder.ItemNameText.text = "?????";
                    ItemList[i].Holder.AmountText.text = "Ilość: " + ItemList[i].ItemAmount;
                    ItemList[i].Holder.IncomeText.text = "Dochód: " + ItemList[i].Item.CalculateIncome(ItemList[i].ItemAmount);
                    ItemList[i].Holder.CostText.text = "Koszt: " + ItemList[i].Item.CalculateCost(ItemList[i].ItemAmount);
                }

                ItemList[i].Holder.BuyButton.Id = i;
                ItemList[i].Created = true;
            }
        }
    }
}