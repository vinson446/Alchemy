using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : State
{
    protected BattleSM StateMachine { get; private set; }

    private void Awake()
    {
        StateMachine = GetComponent<BattleSM>();
    }
}
