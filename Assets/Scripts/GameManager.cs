using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class AnyItem
    {
        [HideInInspector] public int ItemAmount;
        public ClickItem Item;
        public bool Unlocked;
        [HideInInspector] public bool Created;
        [HideInInspector] public ItemHolder Holder;
    }

    public List<AnyItem> ItemList = new List<AnyItem>();
}