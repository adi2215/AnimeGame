using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    public void FinishRace(int placement)
    {
        int reward = placement switch
        {
            1 => 200,
            2 => 100,
            _ => 50
        };

        EventManager.Instance.AddStardust(reward);
        SceneManager.LoadScene("EventHub"); // Return to event screen
    }
}