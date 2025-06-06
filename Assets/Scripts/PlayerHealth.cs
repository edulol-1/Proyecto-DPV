using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Settings")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText; // Cambiado de Text a TextMeshProUGUI

    [Header("Respawn Settings")]
    public Transform[] spawnPoints;
    public float respawnDelay = 2f;
    public int deaths = 0;
    //public Transform playerTrans;


    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is Dead");
        gameObject.SetActive(false);
        Invoke(nameof(Respawn), respawnDelay);
        if (++deaths == 3)
        {
            Invoke(nameof(ShowResults), 2);      
        }
    }

    private void Respawn()
    {
        if (spawnPoints.Length > 0)
        {
            int index = Random.Range(0, spawnPoints.Length);
            //playerTrans.position = spawnPoints[index].position;
            transform.position = spawnPoints[index].position;
        }

        currentHealth = maxHealth;
        UpdateHealthUI();
        gameObject.SetActive(true);
    }

    private void ShowResults()
    {
        string[] results = (gameObject.tag == "Player2")
        ? new string[] { "Player 1", "Player 2" }
        : new string[] { "Player 2", "Player 1" };

        Debug.Log(results[0] + " won the game! :)");
        Debug.Log(results[1] + " lost the game :(");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        SceneManager.LoadScene("Title");
    }
}