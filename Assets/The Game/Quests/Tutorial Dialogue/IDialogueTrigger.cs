using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDialogueTrigger
{
    string[] lines {get; set;}
    bool isTriggered {get; set;}
}