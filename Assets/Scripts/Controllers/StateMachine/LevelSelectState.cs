using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectState : State
{
    protected LevelSelectSM StateMachine { get; private set; }

    private void Awake()
    {
        StateMachine = GetComponent<LevelSelectSM>();
    }
}
