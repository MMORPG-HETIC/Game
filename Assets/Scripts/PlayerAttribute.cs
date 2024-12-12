using System.Net;
using UnityEngine;
using System.Collections;

public class PlayerAttribute : MonoBehaviour
{
    public ClientManager clientManager;
    public string ID;
    private float SendPositionTimeout = -1;
    private Vector3 previousPosition;
    bool canQuit = false;
    private int currentHealth = 100;
    private float lastAttackTime;
    public HealthBar healthBar;

    private void Start()
    {
        clientManager = FindFirstObjectByType<ClientManager>();
        previousPosition = transform.position;
    }

    public bool TakeDamage(int damageAmount)
    {
        lastAttackTime = Time.time;
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    private void Die()
    {
        if (clientManager)
        {
            Destroy(gameObject);
            SendQuitMessage();
        }
    }

    private void Update()
    {
        GameObject zombie = GameObject.FindWithTag("Enemy");
        if (zombie != null)
        {           
            float distance = Vector3.Distance(transform.position, zombie.transform.position);
            if (distance <= 2f && Time.time >= lastAttackTime + 0.5f)
            {
                TakeDamage(20);
            }
        }
        if (string.IsNullOrEmpty(ID)) { return; }

        if (Time.time > SendPositionTimeout && clientManager)
        {
            PayloadPlayerStatus status = new PayloadPlayerStatus
            {
                id = ID
            };

            status.SetPosition(transform.position);
            status.SetRotation(transform.rotation);

            bool isMoving = (transform.position - previousPosition).sqrMagnitude > 0.0001f;
            status.SetIsMoving(isMoving);

            clientManager.SendServerUDPMessage(3, status);

            SendPositionTimeout = Time.time + 0.0333f;
            previousPosition = transform.position;
        }
    }

    void OnApplicationQuit()
    {
        if (!canQuit)
        {
            StartCoroutine(SendQuitMessageAndWait());
        }
    }

    private IEnumerator SendQuitMessageAndWait()
    {
        SendQuitMessage();
        yield return new WaitForSeconds(1f);
        canQuit = true;
    }

    private void SendQuitMessage()
    {
        PayloadCheck quit = new PayloadCheck { id = ID };
        clientManager.SendServerUDPMessage(9, quit);
    }

    private bool WantsToQuit()
    {
        return canQuit;
    }
}

