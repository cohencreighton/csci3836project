using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TowerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 25;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject restartButton;

    private int currentHealth;

    private void Start()
    {
        Time.timeScale = 1f;

        currentHealth = maxHealth;
        UpdateHealthUI();

        gameOverText.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHealthUI()
    {
        healthText.text = "Tower Health: " + currentHealth;
    }

    private void GameOver()
    {
        gameOverText.SetActive(true);
        restartButton.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}