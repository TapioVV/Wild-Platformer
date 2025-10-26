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
    bool playerWon = false;
    [SerializeField] InputActionReference moveToMainMenu;
    [SerializeField] InputActionReference resetNormal;
    [SerializeField] InputActionReference resetOnDeath;
    private void OnEnable()
    {
        moveToMainMenu.action.performed += ToMainMenu;
        resetNormal.action.performed += ResetGame;
        resetOnDeath.action.performed += ResetGameAfterDeath;
    }
    private void OnDisable()
    {
        moveToMainMenu.action.performed -= ToMainMenu;
        resetNormal.action.performed -= ResetGame;
        resetOnDeath.action.performed -= ResetGameAfterDeath;
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
        LoadAScene();
    }
    void ToMainMenu(InputAction.CallbackContext context)
    {
        playerWon = true;
        LoadAScene();
    }
    public void PlayerDied()
    {
        playerIsDead = true;
    }
    public void PlayerWon()
    {
        playerWon = true;
    }
    void LoadAScene()
    {
        if (playerIsDead && triedToReset == false)
        {
            triedToReset = true;
            screenFade.FadeToDark();
        }
        if (playerWon && triedToReset == false)
        {
            triedToReset = true;
            screenFade.FadeToDark();
        }
    }
    public void ReloadThisScene()
    {
        DOTween.KillAll();
        if (triedToReset == true)
        {
            if (playerWon)
            {
                SceneManager.LoadScene(0);
            }
            else if (!playerWon)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

    }
}
