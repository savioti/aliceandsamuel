using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    #region Variables
    public Dialogue[] dialoguesEng;
    public Dialogue[] dialoguesPtBr;
    public Dialogue[] dialogues;
    public SpriteRenderer sisterBalloon;
    public SpriteRenderer sisterTxt;
    public SpriteRenderer brotherBalloon;
    public SpriteRenderer brotherTxt;
    public SpriteRenderer motherBalloon;
    public SpriteRenderer motherTxt;
    private int dialogueIndex;
    private int messageIndex;
    private bool canAdvanceDialog = true;
    private bool isOnConversation = false;
    private Dialogue currentDialogue;
    public static Action OnDialogueStart;
    public static Action OnDialogueEnd;
    public static Action OnAllDialoguesEnd;
    private Coroutine dialogueAdvance;
    #endregion
    private void Start() 
    {
        ToggleHUD(false);
        GameManager.OnDialogueCall += StartDialogue;
        MainMenuManager.OnDialogueCall += StartDialogue;
        EndingManager.OnDialogueCall += StartDialogue;
        Translate();
    }
    private void OnDisable() 
    {
        GameManager.OnDialogueCall -= StartDialogue;
        MainMenuManager.OnDialogueCall -= StartDialogue;
        EndingManager.OnDialogueCall -= StartDialogue;
    }
    private void StartDialogue()
    {
        if (dialogueAdvance != null)
            StopCoroutine(dialogueAdvance);

        if (OnDialogueStart != null) OnDialogueStart();
        EmptyTextBalloons();
        ToggleHUD(true);
        currentDialogue = dialogues[dialogueIndex];
        messageIndex = 0;
        canAdvanceDialog = true;
        isOnConversation = true;
        AdvanceDialogue();
    }
    public void AdvanceDialogue()
    {
        if (!canAdvanceDialog || !isOnConversation) return;

        if (messageIndex >= currentDialogue.GetMessages().Length)
        {
            EndDialogue();
            return;
        }

        StartCoroutine(ButtonCooldown());
        WriteText(currentDialogue);
        messageIndex++;
    }
    private void EndDialogue()
    {
        if (OnDialogueEnd != null) OnDialogueEnd();
        isOnConversation = false;
        dialogueIndex++;
        ToggleHUD(false);

        if (dialogueIndex >= dialogues.Length && OnAllDialoguesEnd != null)
            OnAllDialoguesEnd();
    }
    public void PlayUnscriptedDialogue(Dialogue _dialogue)
    {
        if (isOnConversation) return;

        messageIndex = 0;
        currentDialogue = _dialogue;
        canAdvanceDialog = false;
        isOnConversation = true;
        EmptyTextBalloons();
        ToggleHUD(true);
        dialogueAdvance = StartCoroutine(AutoAdvance());
    }
    private void WriteText(Dialogue _dialogue)
    {
        // DarkenBalloons();
        ToggleHUD(false);
        Message m = _dialogue.GetMessages()[messageIndex];
        string messageOwner = m.GetMessageOwner();

        if (String.Equals(messageOwner, "Sister"))
        {
            LightenBalloon(sisterBalloon);
            sisterTxt.sprite = m.GetMessageImage();
            sisterTxt.enabled = true;
            sisterBalloon.enabled = true;
            // sisterTxt.text = m.GetMessageContent();
        }
        else if (String.Equals(messageOwner, "Brother"))
        {
            LightenBalloon(brotherBalloon);
            brotherTxt.sprite = m.GetMessageImage();
            brotherTxt.enabled = true;
            brotherBalloon.enabled = true;
            // brotherTxt.text = m.GetMessageContent();
        }
        else if (String.Equals(messageOwner, "Mother"))
        {
            LightenBalloon(motherBalloon);
            motherTxt.sprite = m.GetMessageImage();
            motherTxt.enabled = true;
            motherBalloon.enabled = true;
            // motherTxt.text = m.GetMessageContent();
        }
        else
        {
            print("ERROR!");
        }
    }
    private void ToggleHUD(bool toggle)
    {
        sisterTxt.enabled = toggle;
        brotherTxt.enabled = toggle;
        motherTxt.enabled = toggle;
        sisterBalloon.enabled = toggle;
        brotherBalloon.enabled = toggle;
        motherBalloon.enabled = toggle;
    }
    private void DarkenBalloons()
    {
        sisterBalloon.color = Color.gray;
        brotherBalloon.color = Color.gray;
        motherBalloon.color = Color.gray;
    }
    private void LightenBalloon(SpriteRenderer balloon)
    {
        balloon.color = Color.white;
    }
    private void EmptyTextBalloons()
    {
        sisterTxt.sprite = null;
        brotherTxt.sprite = null;
        motherTxt.sprite = null;
    }
    private void Translate()
    {
        if (DataController.Instance.GetIsEnglish())
        {
            dialogues = dialoguesEng;
            print("English selected");
        }
        else
        {
            dialogues = dialoguesPtBr;
            print("Português selecionado");
        }
    }
    private IEnumerator ButtonCooldown() 
    {
        canAdvanceDialog = false;
        yield return new WaitForSeconds(0.5f);
        canAdvanceDialog = true;
    }
    private IEnumerator AutoAdvance() 
    {
        WriteText(currentDialogue);
        messageIndex++;
        yield return new WaitForSeconds(2.75f);

        if (messageIndex >= currentDialogue.GetMessages().Length)
            ToggleHUD(false);
        else
            dialogueAdvance = StartCoroutine(AutoAdvance());
    }
    public bool GetIsOnConversation() {return isOnConversation;}
}
