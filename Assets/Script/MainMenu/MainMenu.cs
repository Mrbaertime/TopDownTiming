using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Play Gameeee");
        SceneManager.LoadScene("MapTest");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Gameeee"); 
        Application.Quit(); 
    }
}
