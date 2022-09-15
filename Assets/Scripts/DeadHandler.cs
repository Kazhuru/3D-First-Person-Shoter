using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class DeadHandler : MonoBehaviour
{
    [SerializeField] SceneLoader loader;
    [SerializeField] UIFader BGFade;
    [SerializeField] UIFader GameoverText;
    [SerializeField] FirstPersonController FPSCtrl;

    public void HandleDead()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        BGFade.FadeIn();
        GameoverText.FadeIn();
        FPSCtrl.enabled = false;
        yield return new WaitForSeconds(3f);
        loader.LoadMainMenu();
    }
}
