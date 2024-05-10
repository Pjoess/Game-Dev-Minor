using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayerNode : IBaseNode
{

    FinalBoss boss;

    public FacePlayerNode( FinalBoss boss )
    {
        this.boss = boss;
    }

    public bool Update()
    {

        if(boss.canRotate)
        {
            Vector3 lookDir = Blackboard.instance.GetPlayerPosition() - boss.transform.position;
            lookDir.Normalize();

            Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
            boss.transform.rotation = Quaternion.RotateTowards(boss.transform.rotation, rotation, boss.rotationSpeed * Time.deltaTime);
        }

        return true;
    }
}
