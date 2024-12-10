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

    private void Start()
    {
        clientManager = FindFirstObjectByType<ClientManager>();
        previousPosition = transform.position;
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(ID)) { return; }

        if (Time.time > SendPositionTimeout)
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

            SendPositionTimeout = Time.time + 0.06f;
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
        Debug.Log("Message envoyé avant de quitter.");
    }

    private bool WantsToQuit()
    {
        return canQuit;
    }
}

