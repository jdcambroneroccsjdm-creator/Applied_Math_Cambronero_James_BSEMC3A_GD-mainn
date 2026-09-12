using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float maxHealth = 100f;
private float currentHealth;


    [Header("Player Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float critRate;
    [SerializeField] private float critDamage;
    
    [SerializeField] private DamageTextUI damageTextPrefab;

    [Header("Temporay values")]
    public float finalDamage;
    public bool isCrit;
    

    [SerializeField] private Animator animator;

   

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentGameState != GameState.Playing)
        {
            return;
        }
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, 0f, v).normalized;

        if (direction != Vector3.zero)
        {
            // Rotate toward movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // Move
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Running animation
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }


    public void DamageEnemy(Enemy enemy)
    {
        var decimalCrit = critRate / 100f;
        var decimalCritDamage = critDamage / 100f;
        isCrit = false;
       

        if (UnityEngine.Random.value < decimalCrit)
        {
            finalDamage = damage * (1f + decimalCritDamage);
            isCrit = true;
        }
        else
        {
            finalDamage = damage;
           
           
        }

        enemy.TakeDamage(finalDamage);

DamageTextUI damageText = Instantiate(damageTextPrefab);
GameObject canvas = GameObject.Find("Canvas");
damageText.transform.SetParent(canvas.transform, false);
damageText.Initialize();
damageText.ShowDamage(finalDamage, isCrit, enemy.transform);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.killedEnemyCount++;
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            GameManager.Instance.AddScore(GameManager.Instance.levelController.giveScore);

            DamageEnemy(enemy);

            
            enemy.OnDied();
        }
    }
}
