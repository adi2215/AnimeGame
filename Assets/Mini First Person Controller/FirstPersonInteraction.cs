using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;  
    public Image crosshair;           

    private Camera _camera;

    public int count = 0;

    public GameObject[] zones;

    public PopUp pop;

    public int countPoints = 0;

    public TextMeshProUGUI textPoints;

    public GameObject bulletPrefab;
    public Transform firePoint;      
    public float bulletForce = 20f;  

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            crosshair.color = Color.red;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Взаимодействие с: " + hit.collider.name);

                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    string obj = interactable.Interact(gameObject);
                    if (obj == "Tree")
                    {
                        count++;

                        if (count == 2)
                        {
                            for (int i = 0; i < zones.Length; i++)
                                zones[i].SetActive(true);
                            
                            countPoints += 10;
                        }
                    }
                    else if (obj == "NPC")
                    {
                        countPoints += 10;
                        Debug.Log("TakeIt");
                    }
                    else if (obj == "AINPC")
                    {
                        countPoints += 10;
                        Debug.Log("TakeIt");
                    }
                    else if (obj == "NPCGUN")
                    {
                        countPoints += 10;
                        Debug.Log("TakeIt");
                    }
                }
            }
        }
        else
        {
            crosshair.color = Color.white;
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            Shoot(hit.collider.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            pop.NextTask();
        }

        textPoints.text = "Points: " + countPoints.ToString();
    }

    void Shoot(GameObject npcEnemy)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        }

        Destroy(npcEnemy, 1f);  
    }
}
