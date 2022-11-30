using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image _fadeImage;
    [SerializeField] GameObject _stageSelect;
    int _stageSelectSceneNumber;
    bool goofyBool = false;

    private void Start()
    {
        _fadeImage.DOFade(0, 0.25f);
    }

    //Fades to black and then loads the next scene
    public void StageSelectMenuButtonPress()
    {
        goofyBool = !goofyBool;
        _stageSelect.SetActive(goofyBool);
    }

    public void StageSelectedButtonPress(int _sceneNumber)
    {
        _stageSelectSceneNumber = _sceneNumber;
        _fadeImage.DOFade(1, 0.25f).OnComplete(LoadStage);
    }
    public void LoadStage()
    {
        SceneManager.LoadScene(_stageSelectSceneNumber);
    }

    

    public void TutorialButtonPress()
    {
        _fadeImage.DOFade(1, 0.25f).OnComplete(LoadTutorialScene);
    }
    void LoadTutorialScene()
    {
        SceneManager.LoadScene(1);
    }

    //Fades to black and then exits the game;
    public void QuitButtonPress()
    {
        _fadeImage.DOFade(1, 0.25f).OnComplete(QuitGame);
    }
    void QuitGame()
    {
        Application.Quit();
    }
}