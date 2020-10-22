using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameSM : StateMachine
{
    [SerializeField] InputController _input;
    public InputController Input => _input;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState<SetupCardGameState>();
    }
}
