using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Message
{
    public string owner;
    [TextArea(3, 5)]
    public string message;
    public Sprite messageImg;
    public string GetMessageOwner() {return owner;}
    public string GetMessageContent() {return message;}
    public Sprite GetMessageImage() {return messageImg;}
}