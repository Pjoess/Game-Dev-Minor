using UnityEngine;

public abstract class QuestStage : MonoBehaviour
{
    [HideInInspector] public string questLogText;
    protected bool isFinished = false;
    protected bool isActive = false;
    abstract public void StartStage();
    abstract public bool CheckStageCompleted();
}
