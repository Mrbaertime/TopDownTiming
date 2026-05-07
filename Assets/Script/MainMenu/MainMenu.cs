using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public Button continueButton;

    void Start()
    {
        if (SaveManager.Instance != null)
        {
            continueButton.interactable =
                SaveManager.Instance.HasSaveFile();
        }
        else
        {
            continueButton.interactable = false;
        }
    }
    public void StartGame()
    {
        Debug.Log("Starting New Game...");
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.DeleteSave(); // ลบเซฟเก่าทิ้ง
            SaveManager.Instance.SetShouldLoad(false); // บอกว่าไม่ต้องโหลด
        }
        SceneManager.LoadScene("MapTest");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Gameeee"); 
        Application.Quit(); 
    }

    public void ContinueGame()
    {
        Debug.Log("Continuing Game...");
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SetShouldLoad(true); // บอกว่าต้องโหลดนะ
        }
        SceneManager.LoadScene("MapTest");
    }
}
