using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("UI Sounds")]
    public AudioClip buttonClick;

    void Awake()
    {
        Instance = this;
    }

    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(buttonClick);
    }
}