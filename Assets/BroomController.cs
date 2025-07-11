using UnityEngine;

public class BroomController : MonoBehaviour,  IInteractable
{
    public string Interact(GameObject gameObject)
    {
        Destroy(this.gameObject, 0.1f);
        return "Broom";
    }
}
