using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Trigger : MonoBehaviour
{
    //0-disabled; 1-active; 2-triggered;
    //0->1 1->2 2->0
    public int status;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        status = 0;
        gameManager = GameManager.Instance;
        init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (status == 1)
        {
            if (detecte())
            {
                triggerEvent();
                status = 2;
            }
        }
    }

    public abstract void init();
    public abstract bool detecte();
    public abstract void triggerEvent();
    public abstract void endTrigger();

}
