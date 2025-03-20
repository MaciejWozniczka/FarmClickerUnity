using UnityEngine;

public class Farmer : MonoBehaviour
{
    int clickAmount = 1;
    void Start()
    {
        
    }

    public void Click()
    {
        GameManager.instance.AddMoney(clickAmount);
    }
}