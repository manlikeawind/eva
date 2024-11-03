using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using System;
using System.Reflection;
using static Menu1Manager;

public class NPC : Interact
{
    public enum Status
    { 
        None, Pre, Ready, Talk, TalkComplete
    }
    private Status status;
    private Status nextStatus;

    private GameManager gameManager;
    private InputCtrl input;
    public GameObject tip;
    private string _name;
    private Dictionary<int, JObject> talkItems;
    private int talkIndex;
    private float statusTime;
    private float talkTime;
    private float talkInterval;
    private int btnSelect;

    private GameObject ui;
    private GameObject txt;
    private GameObject btn0;
    private GameObject btn1;
    private GameObject btn2;
    private GameObject btn3;

    private void Awake()
    {
        talkInterval = 0.05f;
        status = Status.Pre;
        nextStatus = Status.None;
        talkTime = 0f;
        btnSelect = -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        input = InputCtrl.Instance;

        GameObject UIObj = GameObject.FindWithTag("UI");
        Transform uiTrans = UIObj.transform.Find("Conversation");
        ui = uiTrans.gameObject;
        txt = uiTrans.Find("talkBg").Find("text").gameObject;
        btn0 = uiTrans.Find("btnList").Find("button0").gameObject;
        btn0.GetComponent<Button>().onClick.AddListener(delegate () {
            this.btnEvent(0);
        });
        btn1 = uiTrans.Find("btnList").Find("button1").gameObject;
        btn1.GetComponent<Button>().onClick.AddListener(delegate () {
            this.btnEvent(1);
        });
        btn2 = uiTrans.Find("btnList").Find("button2").gameObject;
        btn2.GetComponent<Button>().onClick.AddListener(delegate () {
            this.btnEvent(2);
        });
        btn3 = uiTrans.Find("btnList").Find("button3").gameObject;
        btn3.GetComponent<Button>().onClick.AddListener(delegate () {
            this.btnEvent(3);
        });
    }

    // Update is called once per frame
    void Update()
    {
        statusTime = statusTime + Time.unscaledDeltaTime;
        if (nextStatus != Status.None)
        {
            switch (nextStatus)
            {
                case Status.Talk:
                    talkTime = 0f;
                    btn0.SetActive(false);
                    btn1.SetActive(false);
                    btn2.SetActive(false);
                    btn3.SetActive(false);

                    if(talkIndex < 0) {
                        talkIndex = getFirstTalk();
                    }
                    else
                    {
                        JObject data = (JObject)talkItems[talkIndex];
                        //0 - normal talk, 1 - option talk
                        if ((int)data["type"] == 0)
                        {
                            talkIndex = (int)data["next"];
                        }
                        else
                        {
                            if(btnSelect >= 0)
                            {
                                JObject option = (JObject)data["options"][btnSelect];
                                talkIndex = (int)option["next"];
                                btnSelect = -1;
                            }
                        }
                    }
                    break;
                case Status.TalkComplete:
                    JObject data1 = (JObject)talkItems[talkIndex];
                    if ((int)data1["type"] == 1)
                    {
                        JArray btnList = (JArray)data1["options"];
                        btn0.SetActive(true);
                        btn0.transform.Find("text").GetComponent<TextMeshProUGUI>().text = gameManager.textInfo.getText((string)btnList[0]["option"]);
                        if (btnList.Count > 1) {
                            btn1.SetActive(true);
                            btn1.transform.Find("text").GetComponent<TextMeshProUGUI>().text = gameManager.textInfo.getText((string)btnList[1]["option"]);
                        }
                        if (btnList.Count > 2)
                        {
                            btn2.SetActive(true);
                            btn2.transform.Find("text").GetComponent<TextMeshProUGUI>().text = gameManager.textInfo.getText((string)btnList[2]["option"]);
                        }
                        if (btnList.Count > 3)
                        {
                            btn3.SetActive(true);
                            btn3.transform.Find("text").GetComponent<TextMeshProUGUI>().text = gameManager.textInfo.getText((string)btnList[3]["option"]);
                        }
                        EventSystem.current.SetSelectedGameObject(btn0);
                    }
                    break;
                case Status.Ready:
                    ui.SetActive(false);
                    talkIndex = -1;
                    txt.GetComponent<TextMeshProUGUI>().text = "";
                    break;
            }
            status = nextStatus;
            nextStatus = Status.None;
            statusTime = 0f;
        }

        switch (status)
        {
            case Status.Talk:
                JObject data = (JObject)talkItems[talkIndex];
                string contentid = (string)data["content"];
                string content = gameManager.textInfo.getText(contentid);
                talkTime = talkTime + Time.unscaledDeltaTime;
                if(statusTime > 1f && input.getBtnA() > 0.5f)
                {
                    txt.GetComponent<TextMeshProUGUI>().text = content;
                    nextStatus = Status.TalkComplete;
                }
                if (talkTime > talkInterval)
                {
                    int frame = (int)Math.Floor(statusTime / talkInterval);
                    if(frame < content.Length)
                    {
                        txt.GetComponent<TextMeshProUGUI>().text = content.Substring(0, frame);
                    }
                    else
                    {
                        txt.GetComponent<TextMeshProUGUI>().text = content;
                        nextStatus = Status.TalkComplete;
                    }
                    talkTime = 0f;
                }
                break;
            case Status.TalkComplete:
                if(statusTime > 0.3f)
                {
                    JObject data1 = (JObject)talkItems[talkIndex];
                    if ((int)data1["type"] == 0 && input.consumeBtnADown(0.2f) == true)
                    {
                        int nextTalk = (int)data1["next"];
                        if (nextTalk >= 0)
                        {
                            nextStatus = Status.Talk;
                        }
                        else
                        {
                            nextStatus = Status.Ready;
                        }
                        int e = (int)data1["event"];
                        if(e >= 0)
                        {
                            handleEvent(e);
                        }
                    }
                }
                break;
        }

        /*if (actived == false)
        {
            return;
        }

        if(statusTime > 1f)
        {
            if (input.getBtnA() > 0.5f) {
                statusTime = 0f;
                JObject data = talkItems[talkIndex];
                //0 - normal talk, 1 - option talk
                if ((int)data["type"] == 0)
                {
                    talkIndex = (int)data["next"];
                    if(talkIndex >= 0)
                    {

                        showTalk(talkIndex);
                    }
                    else
                    {
                        stopInteract();
                    }
                }
            }
        }*/
    }

    public void init(JObject info)
    {
        talkIndex = -1;
        talkItems = new Dictionary<int, JObject>();
        _name = (string)info["name"];
        JArray talkItemsJson = (JArray)info["conversation"];
        foreach (var itemJson in talkItemsJson) {
            talkItems.Add((int)itemJson["id"], (JObject)itemJson);
        }
        nextStatus = Status.Ready;
        statusTime = 0f;
    }

    public override void focus()
    {
        tip.SetActive(true); 
    }

    public override void blur()
    {
        tip.SetActive(false);
    }

    public override void startInteract()
    {
        if(status == Status.Ready && statusTime > 1f)
        {
            nextStatus = Status.Talk;
            ui.SetActive(true);
        }
    }

    public override void stopInteract()
    {
        /*statusTime = 0f;
        ui.SetActive(false);
        talkIndex = -1;*/
    }

    public virtual int getFirstTalk() {
        return 0;
    }

    public void btnEvent(int btnIndex) 
    {
        btnSelect = btnIndex;

        JObject option = (JObject)talkItems[talkIndex]["options"][btnIndex];
        int nextTalk = (int)option["next"];
        int eventIndex = (int)option["event"];

        if (eventIndex >= 0)
        {
            handleEvent(eventIndex);
        }

        if (nextTalk >= 0)
        {
            nextStatus = Status.Talk;
        }
        else
        {
            nextStatus = Status.Ready;
        }


        /*if (statusTime < 1f)
        {
            return;
        }
        statusTime = 0f;
        JObject option = (JObject)talkItems[talkIndex]["options"][btnIndex];
        talkIndex = (int)option["next"];
        int eventIndex = (int)option["event"];
        if(eventIndex >= 0)
        {
            handleEvent(eventIndex);
        }
        if(talkIndex >= 0)
        {
            showTalk(talkIndex);
        }
        else
        {
            stopInteract();
        }*/
    }

    public virtual void handleEvent(int eventcode)
    {

    }
}
