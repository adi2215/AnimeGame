using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    private bool isOpen = false;

    public string Interact(GameObject gameObject)
    {
        isOpen = !isOpen;
        return "Tree";
    }
}
