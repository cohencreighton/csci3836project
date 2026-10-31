using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float speed = 10;
    private GameObject newEnemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0.0f, 5.0f);
    }

    private void SpawnEnemy() {
        newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        newEnemy.AddComponent<MoveLeft>().speed = newEnemy.GetComponent<basicEnemy>().speed;
    }

    public class MoveLeft : MonoBehaviour
    {
        public float speed;

        private void Update()
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x < 0.0f) {
                Destroy(gameObject);
            }
        }
    }
}
