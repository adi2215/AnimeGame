using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGun : MonoBehaviour, IInteractable
{
    public enum NPCState { Normal, NormalTwo, Fleeing, Attacking }

    [SerializeField] private float throwForce = 10f;
    public GameObject throuObject;
    public NPCState currentState;
    private GameObject player;
    private Animator animator;
    private bool throwIt = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentState == NPCState.Fleeing)
        {
            LookAtPlayer();
        }

        else if (currentState == NPCState.Attacking)
        {
            if (!throwIt)
                ThrowObject();
            
            Following();
        }
    }

    public void OnPlayerAiming(GameObject p)
    {
        if (currentState == NPCState.Normal)
        {
            player = p;
            currentState = NPCState.Fleeing;
        }

        else if (currentState == NPCState.NormalTwo)
        {
            player = p;
            currentState = NPCState.Attacking;
        }
    }

    private void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            direction.y = 0; 
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void ThrowObject()
    {
        Rigidbody rb = throuObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        throwIt = true;
    }

    public void Following()
    {
        Vector3 targetPosition = player.transform.position - player.transform.forward * 5f;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2f * Time.deltaTime);
        
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1f);
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        animator.SetFloat(Animator.StringToHash("Speed"), Mathf.Lerp(animator.GetFloat("Speed"), 3f, Time.deltaTime * 5f));
        animator.SetFloat(Animator.StringToHash("MotionSpeed"), 1f);
    }

    public string Interact(GameObject gameObject)
    {
        OnPlayerAiming(gameObject);
        return "NPCGUN";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SaveZone") || other.CompareTag("EndZone"))
        {
            Destroy(gameObject);
        }
    }
}
