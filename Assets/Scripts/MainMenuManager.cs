using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Animator boy;
    public Image blackScreen;
    public Image advanceDialogueButton;
    public SpriteRenderer titleSprite;
    public Image startButton;
    public Image creditsButton;
    public Sprite startButtonPtBr;
    public Sprite creditsButtonPtBr;
    private AudioSource audioSource;
    public static System.Action OnDialogueCall;
    public static MainMenuManager Instance {get; private set;}
    private void Awake() 
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        audioSource = GetComponent<AudioSource>();
    }
    private void Start() 
    {
        DialogueManager.OnDialogueEnd += LoadGame;
        Translate();
    }
    private void OnDisable() 
    {
        DialogueManager.OnDialogueEnd -= LoadGame;
    }
    public void StartGame()
    {
        boy.Play("close_book");
        StartCoroutine(FadeAllText());
    }
    private void LoadGame()
    {
        Destroy(GetComponent<DialogueManager>());
        StartCoroutine(FadeToBlack());
    }
    public void CallDialogue()
    {
        if (OnDialogueCall != null) OnDialogueCall();
    }
    private void Translate()
    {
        if (DataController.Instance.GetIsEnglish()) return;

        startButton.sprite = startButtonPtBr;
        creditsButton.sprite = creditsButtonPtBr;
    }
    public void OpenCredits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    private IEnumerator FadeAllText()
    {
        Color color = Color.white;
        int i = 0;

        while (i < 20)
        {
            color.a -= 0.05f;
            startButton.color = color;
            creditsButton.color = color;
            titleSprite.color = color;
            i++;
            yield return new WaitForSeconds(0.025f);
        }
        
        startButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        titleSprite.gameObject.SetActive(false);
        advanceDialogueButton.gameObject.SetActive(true);
    }
}
