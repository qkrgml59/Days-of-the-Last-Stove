using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSecneController : MonoBehaviour
{
    [Header("씬 이름 설정")]
    public string gameSceneName = "GameScene";   // 실제 게임 플레이 씬 이름
    public string titleSceneName = "TitleScene"; // 타이틀 씬 이름

    public GameObject HowToGamePaenl;

    // 게임 시작
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // 게임 다시 시작
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // 타이틀 화면으로 이동
    public void GoToTitle()
    {
        SceneManager.LoadScene("Level_0");
    }

    // 게임 종료
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowHowToPlay()
    {
        HowToGamePaenl.SetActive(true);
    }

    
    public void HideHowToPlay()
    {
        HowToGamePaenl.SetActive(false);
    }
}
