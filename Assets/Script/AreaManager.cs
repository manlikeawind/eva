using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System;
using Newtonsoft.Json.Linq;
using System.IO;

public class AreaManager : MonoBehaviour
{
    //private SqliteHelper sqliteHelper;
    /*private Hashtable prefabs;*/
    //private Hashtable objects;
    //public GameObject root;
    private GameManager gameManager;
    private GameObject hero;
    private int heroFocus;
    private string _name;
    private List<Transform> elementList;
    private List<Transform> interactList;

    private void Start()
    {
        //npc data
        string npcReadPath = Application.streamingAssetsPath + "/npc.json";
        StreamReader npcSr = new StreamReader(npcReadPath);
        string npcStr = npcSr.ReadToEnd();
        JObject npcJsonObj = JObject.Parse(npcStr);



        gameManager = GameManager.Instance;
        hero = GameObject.FindWithTag("Player");
        heroFocus = -1;
        elementList = new List<Transform>();
        interactList = new List<Transform>();
        //root = GameObject.FindGameObjectWithTag("Root");
        /*prefabs = new Hashtable();*/
        //objects = new Hashtable();

        /*        string[] elements = { "fire", "wind" };
                foreach (string e in elements) {
                    string path = "Prefabs/scene/" + e;
                    GameObject prefab = (GameObject)Resources.Load(path);
                    prefabs.Add(e, prefab);
                }*/

        _name = SceneManager.GetActiveScene().name;
        Transform elementParent = transform.Find("element").transform;
        Transform interactParent = transform.Find("interact").transform;
        for (int i = 0; i < elementParent.childCount; i++) {
            elementList.Add(elementParent.GetChild(i));
        }

        for (int i = 0; i < interactParent.childCount; i++)
        {
            Transform t = interactParent.GetChild(i);
            interactList.Add(t);
            if (t.tag.Equals("NPC"))
            {
                string id = t.name;
                JObject data = (JObject)npcJsonObj[id];
                interactParent.GetChild(i).GetComponent<NPC>().init(data);
            }
        }



        gameManager.setSceneLoadComplete();
        gameManager.registerAreaManager(this);
    }

    private void Update()
    {
        //get the closest gameobject 
        float dis = 9999999999f;
        int focus = -1;
        for(int i = 0; i < interactList.Count; i++)
        {
            float x = hero.transform.position.x - interactList[i].position.x;
            float y = hero.transform.position.y - interactList[i].position.y;
            float d = x * x + y * y;
            if(d < dis)
            {
                focus = i;
                dis = d;
            }
        }
        if(dis < 1f)
        {
            if(focus != heroFocus)
            {
                if(heroFocus != -1)
                {
                    interactList[heroFocus].GetComponent<Interact>().blur();
                }
                heroFocus = focus;
                hero.GetComponent<PlayerCtrl>().setInteract(interactList[focus]);
                interactList[focus].GetComponent<Interact>().focus();
            }
        }
        else
        {
            if(heroFocus != -1)
            {
                hero.GetComponent<PlayerCtrl>().setInteract(null);
                interactList[heroFocus].GetComponent<Interact>().blur();
                heroFocus = -1;
            }
        }
    }

    /*public GameObject instGameObject(string name) {
        GameObject instance;
        if (prefabs.ContainsKey(name))
        {
            instance = Instantiate((GameObject)prefabs[name]);
        }
        else
        {
            string path = "Prefabs/scene/" + name;
            GameObject prefab = (GameObject)Resources.Load(path);
            prefabs.Add(name, prefab);
            instance = Instantiate(prefab);
        }
        return instance;
    }*/

    /*    public GameObject getAreaGameObject(string id)
        {
            GameObject obj = (GameObject)objects[id];
            return obj;
        }*/

    public void updateActiveSave() { }
}
