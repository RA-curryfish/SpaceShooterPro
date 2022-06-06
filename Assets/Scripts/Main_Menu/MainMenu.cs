using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Scenes:
    // 0 - Main Menu
    // 1 - Single Player
    // 2 - Co-op
    // 3 - Controls Menu
    [SerializeField] GameObject _audioManager;
    public static bool _musicOn = true;

    void Start()
    {
        if(_musicOn) _audioManager.SetActive(true);
        else _audioManager.SetActive(false);
    }

    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadCoopGame()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadControlsMenu()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleMusic()
    {
        _musicOn=!_musicOn;
        _audioManager.SetActive(_musicOn);
    }
}
