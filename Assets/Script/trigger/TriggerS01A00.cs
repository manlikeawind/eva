using Common;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerS01A00 : Trigger
{
    private int stageIndex;
    private CameraCtrl cam;
    private float statusTime;
    public GameObject npc001;
    public GameObject npc000;
    void Update()
    {
        statusTime = statusTime + Time.unscaledDeltaTime;
        switch (stageIndex)
        {
            case 1:
                if(statusTime < 2f)
                {
                    float scale = BaseParam.cameraSize - 1 * statusTime;
                    cam.scaleCamera(scale);
                }else if(statusTime > 3f)
                {
                    stageIndex = 2;
                    statusTime = 0f;
                    npc001.GetComponent<NPC001>().startInteract();
                }
                break;
            case 2:
                int flag = npc001.GetComponent<NPC001>().flag;
                if(flag == 1)
                {
                    stageIndex = 3;
                    statusTime = 0f;
                    GameObject.FindGameObjectWithTag("UI").transform.Find("Mask").gameObject.SetActive(true);
                    Vector3 pos = new Vector3(-34f, 12.4f, -10f);
                    cam.moveCamera(pos);
                    npc000.transform.position = pos;
                }
                break;
            case 3:
                //black screen,transmission
                if(statusTime > 1f)
                {
                    GameObject.FindGameObjectWithTag("UI").transform.Find("Mask").gameObject.SetActive(false);
                    stageIndex = 4;
                    statusTime = 0f;
                }
                break;
            case 4:
                if(statusTime > 2f)
                {
                    endTrigger();
                }
                break;
        }
    }


    public override void init()
    {
        stageIndex = 0;
        statusTime = 0f;
        cam = gameManager.getCamera();
        int taskStatus = (int)gameManager.gameInfo["data"]["task"]["0"];
        if(taskStatus == 0)
        {
            status = 1;
        }
    }
    public override bool detecte()
    {
        return true;
    }

    public override void triggerEvent()
    {
        Vector3 pos = new Vector3(-24f, 12.4f, -10f);
        stageIndex = 1;
        cam.setCameraMode(0);
        cam.moveCamera(pos);
        gameManager.pauseGame();
    }

    public override void endTrigger()
    {
        cam.setCameraMode(1);
        cam.quickMoveToHero();
        cam.scaleCamera(BaseParam.cameraSize);
        gameManager.resumeGame();
    }
}
