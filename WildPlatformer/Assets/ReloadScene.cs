using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] ScreenFade screenFade;
    bool triedToReset = false;
    bool playerIsDead = false;
    [SerializeField] InputActionReference moveToMainMenu;
    [SerializeField] InputActionReference resetNormal;
    [SerializeField] InputActionReference resetOnDeath;
    private void OnEnable()
    {
        moveToMainMenu.action.started += ToMainMenu;
        resetNormal.action.started += ResetGame;
        resetOnDeath.action.started += ResetGameAfterDeath;
        Player.OnPlayerDeath += PlayerDied;
    }
    private void OnDisable()
    {
        moveToMainMenu.action.started -= ToMainMenu;
        resetNormal.action.started -= ResetGame;
        resetOnDeath.action.started -= ResetGameAfterDeath;
        Player.OnPlayerDeath -= PlayerDied;
    }
    void ResetGame(InputAction.CallbackContext context)
    {
        if (!playerIsDead && triedToReset == false)
        {
            triedToReset = true;
            screenFade.FadeToDark();
        }
    }
    void ResetGameAfterDeath(InputAction.CallbackContext context)
    {
        if (playerIsDead && triedToReset == false)
        {
            triedToReset = true;
            screenFade.FadeToDark();
        }
    }
    void ToMainMenu(InputAction.CallbackContext context)
    {

    }
    void PlayerDied()
    {
        playerIsDead = true;
    }
    public void ReloadThisScene()
    {
        DOTween.KillAll();
        if (triedToReset == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
