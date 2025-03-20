using UnityEngine;

public class Farmer : MonoBehaviour
{
    int clickAmount = 1;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Click()
    {
        GameManager.instance.AddMoney(clickAmount);
        anim.SetTrigger("Click");
    }
}