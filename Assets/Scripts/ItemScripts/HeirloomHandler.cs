using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeirloomHandler : ItemHandlerInterface
{
    public override bool HandleBehavior()
    {
        ShowMonologue(false);
        return false;
    }
}
