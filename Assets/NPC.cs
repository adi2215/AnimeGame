using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour, IInteractable
{
    public Transform player; 
    public float followDistance = 2.5f; 
    public float moveSpeed = 3f; 
    public float smoothRotation = 5f; 
    private bool isFollowing = false; 
    private Animator animator;

    public GameObject image;
    public TextMeshProUGUI textPoints;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isFollowing)
        {
            Vector3 targetPosition = player.position - player.forward * followDistance;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * smoothRotation);
            UpdateAnimations();
        }
    }

    public string Interact(GameObject gameObject)
    {
        player = gameObject.transform;
        Summon();
        return "NPC";
    }

    public void Summon()
    {
        isFollowing = true;
    }

    private void UpdateAnimations()
    {
        animator.SetFloat(Animator.StringToHash("Speed"), Mathf.Lerp(animator.GetFloat("Speed"), 3f, Time.deltaTime * 5f));
        animator.SetFloat(Animator.StringToHash("MotionSpeed"), 1f);
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SaveZone"))
        {
            Destroy(gameObject);
        }

        else if (other.CompareTag("EndZone"))
        {
            image.SetActive(true);
            int countPoints = player.GetComponent<FirstPersonInteraction>().countPoints;

            textPoints.text = "Вы прошли Миссию на отлично + \n + Ваши очки: " + countPoints.ToString();
            Destroy(gameObject, 0.1f);
        }
    }
}
