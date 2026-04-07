using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Time")]
    public float gameDuration = 600f; // 10 นาที
    private float timer;

    [Header("Boss")]
    public float bossInterval = 180f;
    private float bossTimer;

    public GameObject bossPrefab;
    public Transform player;

    [Header("UI")]
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TextMeshProUGUI timerText;

    private bool isGameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timer = gameDuration;
        bossTimer = bossInterval;

        // 👇 ปิด UI ตอนเริ่ม
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameEnded) return;

        timer -= Time.deltaTime;
        bossTimer -= Time.deltaTime;

        UpdateTimerUI();

        if (timer <= 0)
        {
            WinGame();
        }

        if (bossTimer <= 0)
        {
            SpawnBoss();
            bossTimer = bossInterval;
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = minutes + ":" + seconds.ToString("00");
    }

    void SpawnBoss()
    {
        if (bossPrefab == null || player == null) return;

        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 pos = player.position + (Vector3)(dir * 10f);

        Instantiate(bossPrefab, pos, Quaternion.identity);
    }

    public void GameOver()
    {
        if (isGameEnded) return;

        isGameEnded = true;
        Time.timeScale = 0f;

        gameOverPanel.SetActive(true);
    }

    void WinGame()
    {
        isGameEnded = true;
        Time.timeScale = 0f;

        winPanel.SetActive(true);
    }

    // 🔄 ปุ่ม Restart
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}