using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC001 : NPC
{
    public int flag = 0;
    public override int getFirstTalk()
    {
        return 0;
    }

    public override void handleEvent(int eventcode)
    {
        if(eventcode == 0)
        {
            flag = 1;
        }
    }
}
