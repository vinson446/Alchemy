using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameStates { Menu, LevelSelect, Battle};

public class GameManager : MonoBehaviour
{
    [SerializeField] GameStates _gameState;
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
