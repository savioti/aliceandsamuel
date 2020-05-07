using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Credits : MonoBehaviour
{
    public Image creditsImage;
    public float yEndValue = 20f;
    public float timeRolling = 20f;
    public Sprite engThanks;
    public Sprite ptbrThanks;
    private Tween tween;
    private void Start() 
    {
        if (DataController.Instance && !DataController.Instance.GetIsEnglish())
        {
            creditsImage.sprite = ptbrThanks;
        }
        else
        {
            creditsImage.sprite = engThanks;
        }

        tween = creditsImage.transform.DOMoveY(yEndValue, timeRolling).SetEase(Ease.Linear);
    }
    private void OnDisable() 
    {
        tween.Kill();
    }
    public void Return()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
