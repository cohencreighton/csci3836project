using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private int maxHealth = 3;

    [SerializeField] private float attackDistance = 0.8f;
    [SerializeField] private int damage = 1;
    [SerializeField] private bool isFlying = false;
    [SerializeField] private float flashDuration = 0.1f;
    private bool isDying = false;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;
    private EnemySpawner spawner;

    private TowerHealth tower;

    private int currentHealth;

    private void Start()
    {
        spawner = FindAnyObjectByType<EnemySpawner>();
        currentHealth = maxHealth;
        tower = FindAnyObjectByType<TowerHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        if (isDying)
        return;

        Vector3 direction;

        if (isFlying)
        {
            // Flying enemies move toward the tower.
            direction = tower.transform.position - transform.position;
            direction.z = 0f;
        }
        else
        {
            // Ground enemies stay at their current height.
            float horizontalDirection =
                Mathf.Sign(tower.transform.position.x - transform.position.x);

            direction = new Vector3(horizontalDirection, 0f, 0f);
        }   

        transform.position +=
            direction.normalized * speed * Time.deltaTime;

        transform.position +=
            direction.normalized * speed * Time.deltaTime;

        if (tower != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                tower.transform.position
            );

            if (distance <= attackDistance)
            {
                tower.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDying)
        return;

        if (other.GetComponent<Projectile>() != null)
        {
            currentHealth--;
            Destroy(other.gameObject);

            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }

            flashCoroutine = StartCoroutine(FlashWhite());

            if (currentHealth <= 0)
            {
                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                }

                StartCoroutine(Die());
            }
            else
            {
                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                }

                flashCoroutine = StartCoroutine(FlashWhite());
            }
        }
    }

    private IEnumerator Die()
    {
        isDying = true;

        // Stop movement and further collisions.
        Collider2D enemyCollider = GetComponent<Collider2D>();

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

        yield return new WaitForSeconds(0.15f);

        Destroy(gameObject);
    }

    private IEnumerator FlashWhite()
    {
        if (spriteRenderer == null)
            yield break;

        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.EnemyRemoved();
        }
    }
}