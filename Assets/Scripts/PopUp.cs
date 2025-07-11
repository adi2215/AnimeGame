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

    public GameObject obj;
    private string[] tasks = {
        "1. Для передвижения WASD",
        "2. Взаимодействие с Персонажем и Обьектами на кнопку Е",
        "3. NPC : Соберите Метла"
    };
    void Start()
    {
        currentTask = -1;
        //gameObject.SetActive(false);

        StartCoroutine(ShowMessaged());
    }

    public IEnumerator ShowMessaged()
    {
        yield return new WaitForSeconds(1f);
        NextTask();
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
        yield return new WaitForSeconds(2f);
        NextTask();
    }

    public void TogglePopup()
    {
        isVisible = !isVisible;
        //gameObject.SetActive(isVisible);
        animator.SetBool("PopOn", isVisible);
    }

}
