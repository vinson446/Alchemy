using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowLaboratoryState : LevelSelectState
{
    [SerializeField] GameObject[] _allPanels;
    [SerializeField] GameObject _labPanel;

    public override void Enter()
    {
        _labPanel.SetActive(true);

        StateMachine.Input.PressedConfirm += OnPressedLevelSelect;
        StateMachine.Input.PressedGoToMenu += OnPressedMenu;
    }

    public override void Tick()
    {

    }

    public override void Exit()
    {
        foreach (GameObject o in _allPanels)
        {
            _labPanel.SetActive(false);
        }

        StateMachine.Input.PressedConfirm -= OnPressedLevelSelect;
        StateMachine.Input.PressedGoToMenu -= OnPressedMenu;
    }

    public void OnPressedLevelSelect()
    {
        StateMachine.ChangeState<ShowLevelSelectState>();
    }

    public void OnPressedMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
