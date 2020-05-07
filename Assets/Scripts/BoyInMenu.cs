using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MSavioti.UnityExtensionMethods;

public class BoyInMenu : MonoBehaviour
{
    public void BookClose()
    {
        MainMenuManager.Instance.CallDialogue();
    }
    public void PlayBookSound()
    {
        GetComponent<AudioSource>().ForcePlay();
    }
}
