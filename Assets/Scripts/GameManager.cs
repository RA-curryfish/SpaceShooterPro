using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;
    private int _scene;
    
    void Start()
    {
        _scene=SceneManager.GetActiveScene().buildIndex;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_scene==0)   Application.Quit();
            else    SceneManager.LoadScene(0);
        }        
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver)
            SceneManager.LoadScene(_scene);
    }

    public void GameOver()
    {
        _isGameOver=true;
    }
}
