using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightHandler : ItemHandlerInterface
{
    public override bool HandleBehavior()
    {
        ShowMonologue(ItemStatus.NOTUSED, -1);
        return false;
    }
}
