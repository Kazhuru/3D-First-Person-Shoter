using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] AudioClip pointsClip;
    [SerializeField] float pointsClipVolume;
    [SerializeField] float pointsForEnemy = 20;
    [SerializeField] Animator floatingTextAnim;
    [SerializeField] TextMeshProUGUI pointsInfoUI;

    public float currentScore = 0;

    private void Start()
    {
        pointsInfoUI.text = currentScore.ToString();
    }

    public void AddScoreEnemyDead()
    {
        Player.instance.GetAudioSource().PlayOneShot(pointsClip, pointsClipVolume);
        floatingTextAnim.GetComponent<Text>().text =
            "+" + pointsForEnemy + " points added!";
        floatingTextAnim.SetTrigger("startFloat");
        currentScore += pointsForEnemy;
        pointsInfoUI.text = currentScore.ToString();
    }
}
