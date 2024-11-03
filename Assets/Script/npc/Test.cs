using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : NPC
{
    public override int getFirstTalk() {
        return 0;
    }

    public override void handleEvent(int eventcode)
    {
        switch(eventcode)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
}
