public class RunningQuest : QuestStage
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
        questLogText = "Try walking and running \n\n" + $"-> Run along the path.";
        TutorialEvents.OnEnterCamera += Triggered;
    }

    public void Triggered(){
        isFinished = true;
    }
}
