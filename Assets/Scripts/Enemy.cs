using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public bool isMoving;
    private Transform goalTarget;

    [SerializeField] private float maxHealth = 100f;
private float currentHealth;

    private void Start()
    {
        goalTarget = GameObject.FindGameObjectWithTag("Goal").transform;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveToTarget();
        }
        
    }

    public void MoveToTarget()
    {
        Vector3 direction = goalTarget.position - transform.position;

        // Ignore Y movement
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();

            // Move
            transform.position += direction * speed * Time.deltaTime;

            // Rotate toward target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }
    }
    public void OnDied()
    {
        Destroy(gameObject);
       
    }

    public void OnReachGoal()
    {      
        Destroy(gameObject);
        GameManager.Instance.killedEnemyCount++;
        GameManager.Instance.TakeDamage(GameManager.Instance.enemySpawner.damage);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            OnReachGoal();
        }
    }

    public void TakeDamage(float damage)
{
    currentHealth -= damage;

    Debug.Log("Enemy took " + damage + " damage. HP: " + currentHealth);

    if (currentHealth <= 0)
    {
        Die();
    }
}

private void Die()
{
    Destroy(gameObject);
}
}
