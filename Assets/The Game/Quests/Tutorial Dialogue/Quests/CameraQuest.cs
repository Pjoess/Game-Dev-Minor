using UnityEngine;

public class CameraQuest : QuestStage
{
    public GameObject trigger;

    public override bool CheckStageCompleted()
    {
        if(isFinished){
            return true;
        }else{
            return false;
        }
    }

    public override void StartStage()
    {
        isActive = true;
        questLogText = "Try rotating the Camera and the Zoom in and out \n\n" + $"-> Run along the path and move your Camera.";
        trigger.SetActive(false);
        TutorialEvents.OnEnterAttack += Triggered;
    }

    public void Triggered(){
        isFinished = true;
    }
}
