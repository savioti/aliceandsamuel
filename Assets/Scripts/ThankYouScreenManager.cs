using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThankYouScreenManager : MonoBehaviour
{
    public Image thankYouImage;
    public Sprite engThanks;
    public Sprite ptbrThanks;
    private void Start() 
    {
        if (DataController.Instance && !DataController.Instance.GetIsEnglish())
            thankYouImage.sprite = ptbrThanks;
        else
            thankYouImage.sprite = engThanks;
    }
    private void Update() 
    {
        if (Input.anyKey) Application.Quit();
    }
}
