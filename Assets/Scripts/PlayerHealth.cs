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

    public GameObject explosionPrefab;
    public GameObject ufoModel;


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
        currentHealth = 1000;
        ufoModel.SetActive(false);
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        if (++deaths < 3)
        {
            Invoke(nameof(Respawn), 3);
        }
        else
        {
            Invoke(nameof(ShowResults), 3);
        }
    }

    private void Respawn()
    {
        if (spawnPoints.Length > 0)
        {
            int index = Random.Range(0, spawnPoints.Length);
            transform.position = spawnPoints[index].position;
        }
        currentHealth = maxHealth;
        UpdateHealthUI();
        ufoModel.SetActive(true);
    }

    private void ShowResults()
    {
        string winner = (gameObject.tag == "Player2") ? "Player 1" : "Player 2";

        Debug.Log(winner + " won the game! :)");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        SceneManager.LoadScene("Title");
    }
}