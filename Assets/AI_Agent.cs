using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AI_Agent : MonoBehaviour, IInteractable
{
    public enum NPCState
    {
        Normal,
        Fleeing
    }

    [SerializeField] private Transform originalPoint;
    public float fleeSpeed = 6f;
    public float walkSpeed = 3.5f;
    public float idleTime = 3f;
    public float escapeDistance = 100f;
    public float calmDownTime = 1f;

    private NavMeshAgent navAgent;
    private Animator animator;
    private NPCState currentState;
    private GameObject player;

    public GameObject RootObject;

    public bool checking = true;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentState = NPCState.Normal;
        navAgent.speed = walkSpeed;
        navAgent.isStopped = true;
    }

    private void Update()
    {
        UpdateAnimations();
    }

    public string Interact(GameObject gameObject)
    {
        StartFleeing();
        return "AINPC";
    }

    public void StartFleeing()
    {
        if (currentState != NPCState.Fleeing)
        {
            currentState = NPCState.Fleeing;
            navAgent.isStopped = false; 
            StartCoroutine(CalmDown());
            MoveNPCToTargetPoint(originalPoint.position);
        }
    }

    public void StopNPC()
    {
        navAgent.isStopped = true;
        navAgent.ResetPath();
    }

    private void MoveNPCToTargetPoint(Vector3 targetPoint)
    {
        if (navAgent != null)
        {
            navAgent.SetDestination(targetPoint);
            StartCoroutine(CheckArrival(targetPoint));
        }
    }

    private IEnumerator CheckArrival(Vector3 targetPoint)
    {
        while (navAgent != null && Vector3.Distance(transform.position, targetPoint) > 1f)
        {
            yield return null; // Ждём следующий кадр
        }
    }

    private void UpdateAnimations()
    {
        float targetSpeed = (currentState == NPCState.Fleeing) ? fleeSpeed : 0f;
        animator.SetFloat(Animator.StringToHash("Speed"), Mathf.Lerp(animator.GetFloat("Speed"), targetSpeed, Time.deltaTime * 5f));
        animator.SetFloat(Animator.StringToHash("MotionSpeed"), 1f);

        if (navAgent != null && Vector3.Distance(transform.position, originalPoint.position) < 0.1f && checking)
        {
            checking = false;
            StartCoroutine(CalmDown());
        }
    }

    IEnumerator CalmDown()
    {
        yield return new WaitForSeconds(calmDownTime);
        currentState = NPCState.Normal;
        StopNPC();
    }
}
