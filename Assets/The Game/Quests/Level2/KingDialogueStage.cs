using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KingDialogueStage : QuestStage
{
    [SerializeField] private BlackScreenScript blackScreen;
    [SerializeField] List<GameObject> objectsToDisable = new List<GameObject>();
    private DialogueManager manager;

    public override void StartStage()
    {
        isActive = true;
        blackScreen.EnableBlackScreen();
        GetComponent<Dialogue>().TriggerDialgue();
        manager = FindObjectOfType<DialogueManager>();
        Blackboard.instance.DisablePlayerInput();
    }

    public override bool CheckStageCompleted()
    {
        if (isFinished)
        {
            var player = FindObjectOfType<Player_Manager>().gameObject;
            player.transform.position = new Vector3(535.659973f, 0.178000003f, 463.100006f);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);

            var cam = FindObjectOfType<CinemachineVirtualCamera>();
            cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.Value = -90;

            foreach ( var obj in objectsToDisable)
            {
                obj.SetActive(false);
            }

            isActive = false;
            blackScreen.DisableBlackScreen();
            Blackboard.instance.EnablePlayerInput();
            return true;
        }
        else
        {
            CheckDialogueOver();
            return false;
        }
    }

    private void CheckDialogueOver()
    {
        if (!manager.isActive) isFinished = true;
    }
}
