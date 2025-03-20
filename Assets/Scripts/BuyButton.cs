using UnityEngine;

public class BuyButton : MonoBehaviour
{
    [HideInInspector]
    public int Id;
    public void BuyAnItem()
    {
        GameManager.instance.BuyItem(Id);
    }
}