using UnityEngine;

public class Farmer : MonoBehaviour
{
    int clickAmount = 1;

    Animator anim;
    public GameObject popUpTextPrefab;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Click()
    {
        GameManager.instance.AddMoney(clickAmount);
        anim.SetTrigger("Click");

        GameObject pop = Instantiate(popUpTextPrefab, this.transform, false) as GameObject;
        pop.transform.position = Input.mousePosition;

        pop.GetComponent<PopUp>().ShowInfo(clickAmount);
    }
}