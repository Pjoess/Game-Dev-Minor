using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [HideInInspector] public enum DialogueCharacter { BUDDY, ARTHUR, KING }
    public DialogueCharacter character;
    public string line;
    public AudioClip audioClip;
}
