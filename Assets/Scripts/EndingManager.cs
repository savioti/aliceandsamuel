using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public Image blackScreen;
    public static System.Action OnDialogueCall;
    private void Start() 
    {
        blackScreen.gameObject.SetActive(false);
        DialogueManager.OnDialogueEnd += EndScene;
    }
    private void OnDisable() 
    {
        DialogueManager.OnDialogueEnd -= EndScene;
    }
    public void CallDialogue()
    {
        if (OnDialogueCall != null) OnDialogueCall();
    }
    private void EndScene()
    {
        StartCoroutine(FadeToBlack());
    }
    private IEnumerator FadeToBlack()
    {
        Color color = Color.black;
        color.a = 0;
        blackScreen.color = color;
        blackScreen.gameObject.SetActive(true);
        int i = 0;

        while (i < 20)
        {
            color = blackScreen.color;
            color.a += 0.05f;
            blackScreen.color = color;
            i++;
            yield return new WaitForSeconds(0.1f);
        }

        blackScreen.color = Color.black;

        UnityEngine.SceneManagement.SceneManager.LoadScene("ThanksForPlaying");
    }
}
