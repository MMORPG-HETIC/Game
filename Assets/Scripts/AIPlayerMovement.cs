using UnityEngine;

public class AIPlayerMovement : MonoBehaviour
{
    public float speed = 3f; // Vitesse de déplacement de l'IA
    public float changeDirectionInterval = 2f; // Temps avant de changer de direction

    private Vector3 direction;
    private float timeSinceDirectionChange = 0;

    void Start()
    {
        ChangeDirection();
    }

    void Update()
    {
        timeSinceDirectionChange += Time.deltaTime;

        // Changer de direction après l'intervalle défini
        if (timeSinceDirectionChange >= changeDirectionInterval)
        {
            ChangeDirection();
        }

        // Déplacer l'IA
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void ChangeDirection()
    {
        // Générer une nouvelle direction aléatoire
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        timeSinceDirectionChange = 0;
    }
}
