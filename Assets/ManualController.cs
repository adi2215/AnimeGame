using UnityEngine;
using TMPro;

public class ManualController : MonoBehaviour
{
    //public GameObject manualPanel;
    public TextMeshProUGUI manualText;
    public Animator animator;
    private bool isManualOpen = false;
    public int currentTask = 0;
    private string[] tasks = {
        "Для чтобы...",
        "используйте желтую ленту...",
        "воспользуйтесь громкоговорителем",
        "будьте осторожны"
    };

    private void Start()
    {
        NextTask();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleManual();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ToggleManual();
        }
    }


    public void NextTask()
    {
        currentTask += 1;
        manualText.text = tasks[currentTask];
    }

    void ToggleManual()
    {
        animator.SetTrigger("ToggleManual");
    }
}
