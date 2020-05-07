using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{
    private bool badEnding;
    private bool isInEnglish = true;
    public static DataController Instance { get; private set; }
    private void Awake() 
    {
        if (Instance != null && Instance != this)
            Destroy (this.gameObject);
        else
            Instance = this;
    }
    private void Start() 
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SetIsEnglish(bool set)
    {
        print(set);
        isInEnglish = set;
        print(isInEnglish);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public bool GetIsEnglish() {return isInEnglish;}
}
