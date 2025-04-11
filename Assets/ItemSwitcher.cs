using UnityEngine;

public class ItemSwitcher : MonoBehaviour
{
    public GameObject[] items; 
    private int currentIndex = 0;

    void Start()
    {
        UpdateItemVisibility();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            NextItem();
        }
        else if (scroll < 0f)
        {
            PreviousItem();
        }
    }

    void NextItem()
    {
        currentIndex = (currentIndex + 1) % items.Length;
        UpdateItemVisibility();
    }

    void PreviousItem()
    {
        currentIndex = (currentIndex - 1 + items.Length) % items.Length;
        UpdateItemVisibility();
    }

    void UpdateItemVisibility()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(i == currentIndex);
        }
    }
}
