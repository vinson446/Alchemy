using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamagePlayEffect",
                 menuName = "CardData/PlayEffects/Gold")]
public class Gold : CardPlayEffect
{
    GameManager _gameManager;

    public override void Activate()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _gameManager.IncrementGold(1000);
    }
}
