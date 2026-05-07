using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // เพิ่มอันนี้เพื่อใช้เช็ค Scene

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string savePath;
    public bool shouldLoad { get; private set; }

    [Header("References")]
    public PlayerLevel playerLevel;
    public Health playerHealth;
    public AbilityManager abilityManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        savePath = Application.persistentDataPath + "/save.json";
    }

    // =========================
    // SCENE LOAD EVENT (ระบบใหม่แทน AutoLoadGame)
    // =========================
    void OnEnable()
    {
        // สมัครรับ Event เมื่อมีการโหลดฉากใหม่
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // ยกเลิก Event เมื่อสคริปต์ถูกลบ
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ถ้าโหลดฉาก MapTest เสร็จ ให้รอ 1 เฟรมแล้วค่อยดึงข้อมูล (กันบัคโดน Start() รีเซ็ตทับ)
        if (scene.name == "MapTest")
        {
            StartCoroutine(DelayedLoadSetup());
        }
    }

    IEnumerator DelayedLoadSetup()
    {
        // รอ 1 เฟรม ให้ Player, Health ฯลฯ ทำงาน Start() ให้เสร็จก่อน
        yield return new WaitForEndOfFrame();

        FindReferences(); // หาตัวละครในฉากใหม่

        if (shouldLoad && HasSaveFile())
        {
            Debug.Log("ฉากโหลดเสร็จแล้ว ทำการโหลด Save...");
            LoadGame();
        }
        else
        {
            Debug.Log("นี่คือการเริ่มเกมใหม่ ไม่โหลด Save");
        }
    }

    public void SetShouldLoad(bool value)
    {
        shouldLoad = value;
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("ลบเซฟเก่าทิ้งแล้ว");
        }
    }

    // =========================
    // FIND REFERENCES
    // =========================
    void FindReferences()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerLevel = player.GetComponent<PlayerLevel>();
            playerHealth = player.GetComponent<Health>();
        }
        else
        {
            Debug.LogError("หา Player ไม่เจอ! ลืมใส่ Tag 'Player' หรือเปล่า?");
        }

        abilityManager = FindFirstObjectByType<AbilityManager>();
    }

    // =========================
    // SAVE & LOAD
    // =========================
    public void SaveGame()
    {
        if (playerLevel == null || playerHealth == null || abilityManager == null) FindReferences();

        SaveData data = new SaveData();

        data.level = playerLevel.level;
        data.currentExp = playerLevel.currentExp;
        data.expToNext = playerLevel.expToNext;
        data.currentHP = playerHealth.GetCurrentHealth();

        foreach (var ab in abilityManager.GetOwnedAbilities())
        {
            data.ownedAbilities.Add(ab.abilityName);
        }

        //เพิ่มการเซฟเวลา โดยดึงจาก GameManager
        if (GameManager.Instance != null)
        {
            data.timer = GameManager.Instance.timer;
            data.bossTimer = GameManager.Instance.bossTimer;
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath)) return;

        if (playerLevel == null || playerHealth == null || abilityManager == null) FindReferences();

        // เช็คอีกรอบเพื่อความชัวร์ว่าหาตัวละครเจอจริงๆ จะได้ไม่ Error
        if (playerLevel == null)
        {
            Debug.LogError("หยุดโหลด! หา Player Level ไม่เจอ ข้อมูลจะไม่ถูกนำไปใช้");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // APPLY DATA
        playerLevel.level = data.level;
        playerLevel.currentExp = data.currentExp;
        playerLevel.expToNext = data.expToNext;

        playerHealth.SetCurrentHealth(data.currentHP);
        abilityManager.LoadAbilities(data.ownedAbilities);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.timer = data.timer;
            GameManager.Instance.bossTimer = data.bossTimer;
        }

        Debug.Log("โหลดข้อมูลสำเร็จ เลเวล: " + data.level + " เวลาเหลือ: " + data.timer);
    }

    public bool HasSaveFile()
    {
        return File.Exists(savePath);
    }
}