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
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI remainingLivesGUI;
    public TextMeshProUGUI finalStatusPlayer;
    public TextMeshProUGUI finalStatusRival;
    private int remLives = 3;

    [Header("Respawn Settings")]
    public Transform[] spawnPoints;
    public float respawnDelay = 3f;

    [Header("Sound Effects")]
    public AudioClip explosionSound;
    public AudioClip damageSound;
    public float explosionVolume = 1.0f;
    public float damageVolume = 0.7f;

    private AudioSource audioSource;

    public GameObject explosionPrefab;
    public GameObject ufoModel;

    private void Start()
    {
        currentHealth = maxHealth;
        
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        
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

        if (remainingLivesGUI != null)
        {
            remainingLivesGUI.text = $"{remLives}";
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        
        if (currentHealth > 0 && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound, damageVolume);
        }

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
        
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound, explosionVolume);
        }
        else
        {
            Debug.LogWarning("Missing explosion sound or AudioSource!");
        }
        
        if (--remLives > 0)
        {
            Invoke(nameof(Respawn), respawnDelay);
        }
        else
        {
            ShowResults();
        }
    }

    private void Respawn()
    {
        if (spawnPoints.Length > 0)
        {
            CharacterController cc = GetComponent<CharacterController>();
            cc.enabled = false;
            int index = Random.Range(0, spawnPoints.Length);
            transform.position = spawnPoints[index].position;
            cc.enabled = true;
        }

        currentHealth = maxHealth;
        UpdateHealthUI();
        ufoModel.SetActive(true);
    }

    private void ShowResults()
    {
        finalStatusPlayer.text = "You lose";
        finalStatusRival.text = "You win!";
        Invoke(nameof(GetBackToMenu), 6);
    }

    private void GetBackToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }
}