using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] GameObject stageSelect;
    int stageSelectSceneNumber;
    bool goofyBool = false;

    private void Start()
    {
        fadeImage.DOFade(0, 0.25f);
    }

    //Fades to black and then loads the next scene
    public void StageSelectMenuButtonPress()
    {
        goofyBool = !goofyBool;
        stageSelect.SetActive(goofyBool);
    }

    public void StageSelectedButtonPress(int sceneNumber)
    {
        stageSelectSceneNumber = sceneNumber;
        fadeImage.DOFade(1, 0.25f).OnComplete(LoadStage);
    }
    public void LoadStage()
    {
        SceneManager.LoadScene(1 + stageSelectSceneNumber);
    }



    public void TutorialButtonPress()
    {
        fadeImage.DOFade(1, 0.25f).OnComplete(LoadTutorialScene);
    }
    void LoadTutorialScene()
    {
        SceneManager.LoadScene(1);
    }

    //Fades to black and then exits the game;
    public void QuitButtonPress()
    {
        fadeImage.DOFade(1, 0.25f).OnComplete(QuitGame);
    }
    void QuitGame()
    {
        Application.Quit();
    }
}