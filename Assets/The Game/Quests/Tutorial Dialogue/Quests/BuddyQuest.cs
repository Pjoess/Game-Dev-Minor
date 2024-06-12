using UnityEngine;

public class BuddyQuest : QuestStage
{
    public GameObject trigger;
    public GameObject nextTrigger;
    public int killedSlimes;

    public override bool CheckStageCompleted()
    {
        UpdateText();
        if(isFinished){
            nextTrigger.GetComponent<Collider>().isTrigger = true;
            return true;
        }else{
            return false;
        }
    }

    public override void StartStage()
    {
        killedSlimes = 0;
        isActive = true;
        questLogText = "Try using the buddy attacks \n\n" + $"-> Run along the path and kill the slimes. When you are done, enter the portal.\n -> {killedSlimes}/3";
        nextTrigger.GetComponent<Collider>().isTrigger = false;
        trigger.SetActive(false);
        TutorialEvents.OnKillSlimes += Triggered;
    }

    public void UpdateText(){
        questLogText = "Try using the buddy attacks \n\n" + $"-> Run along the path and kill the slimes. When you are done, enter the portal.\n -> {killedSlimes}/3";
    }

    public void Triggered(){
        if(killedSlimes < 2){
            killedSlimes++;
        }else{
            isFinished = true;
        }
    }
}
