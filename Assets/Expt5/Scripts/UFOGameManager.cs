using SoodUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class UFOGameManager : GenericSingletonClass<UFOGameManager>
{
    public delegate void ScoreUpdatedDelegate(int score);

    public RawImage map;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI healthUI;

    public ScoreUpdatedDelegate OnScoreUpdateCallback;
    public int score = 0;

    public Health playerHealth;

    public void AddToScore(int delta)
    {
        score += delta;

        scoreUI.text = "Score: " + score;
    }

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            score = 0;
            AddToScore(0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            map.gameObject.SetActive(!map.gameObject.activeSelf);
        }

        healthUI.text = "HP: " + playerHealth.currentHealth;
    }
}
