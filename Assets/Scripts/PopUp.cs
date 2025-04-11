using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class PopUp : MonoBehaviour
{
    public TextMeshProUGUI taskText;
    public Animator animator;
    private bool isVisible = false;
    public int currentTask = -1;
    private string[] tasks = {
        "1. Попросите посторонних покинуть местность",
        "2. Оцепите территорию",
        "3. Освободите заложников",
        "4. Обезвредить преступников"
    };
    void Start()
    {
        currentTask = -1;
        //gameObject.SetActive(false);
    }

    public void ShowMessage(string s)
    {
        taskText.text = s;
        StartCoroutine(AnimatePopUp());
    }

    public void NextTask()
    {
        currentTask += 1;
        taskText.text = tasks[currentTask];
        StartCoroutine(AnimatePopUp());
    }

    public IEnumerator AnimatePopUp()
    {
        TogglePopup();
        yield return new WaitForSeconds(4f);
        TogglePopup();
    }

    public void TogglePopup()
    {
        isVisible = !isVisible;
        //gameObject.SetActive(isVisible);
        animator.SetBool("PopOn", isVisible);
    }

}
