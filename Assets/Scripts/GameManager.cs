using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
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
    public int Money;
    public Text TotalMoneyText;
    public Text TotalIncomeText;

    // User Interface
    [Header("USER INTERFACE")]
    public GameObject ItemHolder;
    public Transform Grid;

    // Initialization
    void Awake()
    {
        instance = this;   
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("IdleSave"))
        {
            LoadGame();
        }
        else
        {
            FillList();
        }

        UpdateMoneyUI();
        UpdateIncomeUI();
        StartCoroutine(Tick());
        AutoSave();
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
                    Money = (int)Mathf.Round(Money);

                    UpdateMoneyUI();
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

                FillItemList(i);
            }
        }
    }
    void FillSingleItem(int i)
    {
        if (ItemList[i].Unlocked)
        {
            FillItemList(i);
        }
    }

    private void FillItemList(int i)
    {
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

    // UI
    public void BuyItem(int id)
    {
        if (Money < ItemList[id].Item.CalculateCost(ItemList[id].ItemAmount))
        {
            Debug.Log("Not enough money");
            return;
        }

        Money -= ItemList[id].Item.CalculateCost(ItemList[id].ItemAmount);

        // UPDATE UI IN HOLDER
        if (ItemList[id].ItemAmount < 1)
        {
            ItemList[id].Holder.itemImage.sprite = ItemList[id].Item.ItemImage;
            ItemList[id].Holder.ItemNameText.text = ItemList[id].Item.ItemName;
        }

        ItemList[id].ItemAmount++;
        ItemList[id].Holder.AmountText.text = "Ilość: " + ItemList[id].ItemAmount;
        ItemList[id].Holder.IncomeText.text = "Dochód: " + ItemList[id].Item.CalculateIncome(ItemList[id].ItemAmount);
        ItemList[id].Holder.CostText.text = "Koszt: " + ItemList[id].Item.CalculateCost(ItemList[id].ItemAmount);
            
        // UNLOCK NEXT DRINK
        if (id < ItemList.Count - 1 && ItemList[id].ItemAmount > 0)
        {
            ItemList[id + 1].Unlocked = true;
            FillList();
        }

        UpdateMoneyUI();
        UpdateIncomeUI();
    }
    public void AddMoney(int clickAmount)
    {
        Money += clickAmount;

        UpdateMoneyUI();
    }
    void UpdateMoneyUI()
    {
        TotalMoneyText.text = "Total Money: " + Money.ToString();
    }
    void UpdateIncomeUI()
    {
        float totalIncome = 0;

        foreach(AnyItem item in ItemList)
        {
            if (item.ItemAmount > 0)
            {
                totalIncome += item.Item.CalculateIncome(item.ItemAmount);
            }
        }

        TotalIncomeText.text = "Total Income: " + totalIncome;
    }
    void SaveGame()
    {
        var saveModel = new SaveLoad.SaveModel()
        {
            Money = Money
        };

        for (int i = 0; i < 10; i++)
        {
            var prop = typeof(SaveLoad.SaveModel).GetProperty($"Item{i}");

            if (prop != null && i < ItemList.Count)
            {
                prop.SetValue(saveModel, ItemList[i].ItemAmount);
            }
        }
        
        SaveLoad.Save(saveModel);
    }
    void AutoSave()
    {
        SaveGame();
        Invoke("AutoSave", 60f);
    }
    void LoadGame()
    {
        if (PlayerPrefs.HasKey("IdleSave"))
        {
            string data = SaveLoad.Load();

            string[] dataList = data.Split("|" [0]);

            for (int i = 0; i < dataList.Length-1; i++)
            {
                int temp = int.Parse(dataList[i]);
                ItemList[i].ItemAmount = temp;

                if (temp > 0 && i + 1 < ItemList.Count)
                {
                    ItemList[i + 1].Unlocked = true;
                }

                FillSingleItem(i);
            }

            Money = int.Parse(dataList.LastOrDefault());

            FillList();

            UpdateMoneyUI();
            UpdateIncomeUI();
        }
    }
}