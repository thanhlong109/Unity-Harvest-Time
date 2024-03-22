using Assets.Scripts.DataService;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string musicName;
    private void Start()
    {
        AudioManager.Instance.PlayMusic(musicName);
    }

    public void PlayGame()
    { 
        ScreenPara.Instance.isContinue = false;
        SceneManager.LoadScene(1);
       
    }

    public void ContinueGame()
    {
        ScreenPara.Instance.isContinue = true;
        SceneManager.LoadScene(1);

    }


    public void QuitGame()
    {
        Application.Quit();
    }

}
