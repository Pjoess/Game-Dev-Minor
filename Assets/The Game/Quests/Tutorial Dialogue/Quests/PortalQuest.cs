using UnityEngine;

public class PortalQuest: QuestStage
{

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
        questLogText = "Get to level 1 \n\n" + $"-> Run into the portal.";
    }

    public void Triggered(){
        isFinished = true;
    }
}
