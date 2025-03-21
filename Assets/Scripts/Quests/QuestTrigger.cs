using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface QuestTrigger
{
    public abstract bool getCompleted();
    public abstract string getStatus();
    public abstract void updateCheck();

    public abstract void forceComplete();
}
