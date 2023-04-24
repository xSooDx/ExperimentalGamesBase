using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SoodUtils;
using TMPro;

public class HUDManager : GenericSingletonClass<HUDManager>
{
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Slider healthBar;
    [SerializeField] CanvasGroup pauseMenu;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] FollowTarget ft;

    bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.alpha = 0f;
        sensitivitySlider.value = ft.lookSensitivity;
        sensitivitySlider.onValueChanged.AddListener((value) =>
        {
            ft.lookSensitivity = Mathf.Clamp01(value);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResume();
        }
    }

    public void PauseResume()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;
        pauseMenu.alpha = isPaused ? 1.0f : 0.0f;

        Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    public void UpdateSpeedUI(float speed)
    {
        speedText.text = speed.ToString("0.00");
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        healthBar.value = health / maxHealth;
        healthText.text = $"{(int)health}/{(int)maxHealth}";
    }

    

    public void Exit()
    {
        Application.Quit();
    }
}
