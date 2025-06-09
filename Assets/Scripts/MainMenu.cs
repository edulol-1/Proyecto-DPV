using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Necesario para EventSystem

public class MainMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject playButton; // Asigna el botón "Play" desde el Inspector
    public GameObject exitButton; // Asigna el botón "Exit" desde el Inspector

    [Header("Controller Settings")]
    public float navigationCooldown = 0.2f; // Evita cambios rápidos no deseados
    private float cooldownTimer = 0;
    private bool isPlaySelected = true; // Empezamos con "Play" seleccionado

    void Start()
    {
        // Selecciona el botón "Play" al inicio
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        // Navegación con el stick izquierdo (eje vertical)
        float verticalInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(verticalInput) > 0.5f) // Umbral para evitar sensibilidad no deseada
        {
            if (isPlaySelected)
            {
                EventSystem.current.SetSelectedGameObject(exitButton);
                isPlaySelected = false;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(playButton);
                isPlaySelected = true;
            }
            cooldownTimer = navigationCooldown;
        }

        // Confirmar con el botón A (Fire1) o Enter (Submit)
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1"))
        {
            if (isPlaySelected)
            {
                PlayGame();
            }
            else
            {
                QuitGame();
            }
        }

        // Cancelar con el botón B (Fire2) o Escape
        if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}