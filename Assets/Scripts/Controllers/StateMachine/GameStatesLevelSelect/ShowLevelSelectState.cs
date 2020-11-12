using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShowLevelSelectState : LevelSelectState
{
    [SerializeField] GameObject[] _allPanels;
    [SerializeField] Transform[] thumbNails;
    [SerializeField] GameObject _levelSelectPanel;

    [SerializeField] GameObject[] _allLevels;
    [SerializeField] Button[] _sideButtons;
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] TextMeshProUGUI _stageText;

    public int _currentStageIndex;
    public int _previousStageIndex;

    public override void Enter()
    {
        GoToCurrentLevel();

        _levelSelectPanel.SetActive(true);

        /*
        if (gameManager.CurrentLevel == 0)
            DisableLeftArrow();
        else if (gameManager.CurrentLevel == 8)
            DisableRightArrow();
        */

        // StateMachine.Input.PressedViewLab += OnPressedLab;
        StateMachine.Input.PressedViewDeck += OnPressedDeck;

        StateMachine.Input.PressedGoToMenu += OnPressedMenu;
        StateMachine.Input.PressedGoToBattle += OnPressedBattle;

        // StateMachine.Input.PressedRight += RightArrow;
        // StateMachine.Input.PressedLeft += LeftArrow;
    }

    public override void Tick()
    {
        /*
        for (int i = 0; i < _allLevels.Length; i++)
        {
            if (i < gameManager.LevelsUnlocked)
                _allLevels[i].GetComponent<Button>().interactable = true;
            else
                _allLevels[i].GetComponent<Button>().interactable = false;
        }
        */
    }

    public override void Exit()
    {
        foreach (GameObject o in _allPanels)
        {
            _levelSelectPanel.SetActive(false);
        }

        // StateMachine.Input.PressedViewLab -= OnPressedLab;
        StateMachine.Input.PressedViewDeck -= OnPressedDeck;

        StateMachine.Input.PressedGoToMenu -= OnPressedMenu;
        StateMachine.Input.PressedGoToBattle -= OnPressedBattle;

        // StateMachine.Input.PressedRight -= RightArrow;
        // StateMachine.Input.PressedLeft -= LeftArrow;
    }

    void OnPressedLab()
    {
        StateMachine.ChangeState<ShowLaboratoryState>();
    }

    void OnPressedDeck()
    {
        StateMachine.ChangeState<ShowDeckState>();
    }

    void OnPressedBattle()
    {
        GameManager._instance.SelectLevel(_currentStageIndex);
        SceneManager.LoadScene("Battle");
    }

    void OnPressedMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void GoToCurrentLevel()
    {
        for (int i = 0; i < GameManager._instance.CurrentLevel; i++)
        {
            RightArrow(false);
        }
    }

    public void RightArrow(bool click)
    {
        if ((_currentStageIndex < _allLevels.Length - 1) && (_currentStageIndex < GameManager._instance.LevelsUnlocked))
        {
            // _sideButtons[1].gameObject.SetActive(true);

            _previousStageIndex = _currentStageIndex;
            _currentStageIndex += 1;

            // move panels
            for (int i = _allLevels.Length - 1; i > 0; i--)
            {
                if (!click)
                    _allLevels[i].transform.position = _allLevels[i - 1].transform.position;
                else
                    _allLevels[i].transform.DOMoveX(_allLevels[i - 1].transform.position.x, 0.25f, true);

                _allLevels[i - 1].GetComponent<Button>().interactable = false;
            }
            if (!click)
                _allLevels[0].transform.position -= new Vector3(750, 0, 0);
            else
                _allLevels[0].transform.DOMoveX(_allLevels[0].transform.position.x - 750, 0.25f, true);

            _allLevels[_currentStageIndex].GetComponent<Button>().interactable = true;

            MinimizePreviousLevel();
            EnlargeCurrentLevel();

            StartCoroutine(DisableRightLeft());

            UpdateLevelStageText();
        }
        /*
        if (_currentStageIndex == 8)
        {
            DisableRightArrow();
        }
        */
    }

    public void DisableRightArrow()
    {
        _sideButtons[0].gameObject.SetActive(false);
    }

    public void LeftArrow()
    {
        if (_currentStageIndex > 0)
        {
            // _sideButtons[0].gameObject.SetActive(true);

            _previousStageIndex = _currentStageIndex;
            _currentStageIndex -= 1;

            for (int i = 0; i < _allLevels.Length - 1; i++)
            {
                _allLevels[i].transform.DOMoveX(_allLevels[i + 1].transform.position.x, 0.25f, true);
                _allLevels[i + 1].GetComponent<Button>().interactable = false;
            }
            _allLevels[_allLevels.Length - 1].transform.DOMoveX(_allLevels[_allLevels.Length - 1].transform.position.x + 750, 0.25f, true);

            _allLevels[_currentStageIndex].GetComponent<Button>().interactable = true;

            MinimizePreviousLevel();
            EnlargeCurrentLevel();

            StartCoroutine(DisableRightLeft());

            UpdateLevelStageText();
        }
        /*
        if (_currentStageIndex == 0)
        {
            DisableLeftArrow();
        }
        */
    }

    public void DisableLeftArrow()
    {
        _sideButtons[1].gameObject.SetActive(false);
    }

    void MinimizePreviousLevel()
    {
        _allLevels[_previousStageIndex].transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400);
        _allLevels[_previousStageIndex].transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600);
        _allLevels[_previousStageIndex].transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300);
        _allLevels[_previousStageIndex].transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
    }

    void EnlargeCurrentLevel()
    {
        _allLevels[_currentStageIndex].transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 600);
        _allLevels[_currentStageIndex].transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 800);
        _allLevels[_currentStageIndex].transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500);
        _allLevels[_currentStageIndex].transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);
    }

    IEnumerator DisableRightLeft()
    {
        foreach(Button b in _sideButtons)
        {
            b.interactable = false;
        }

        yield return new WaitForSeconds(0.25f);

        foreach (Button b in _sideButtons)
        {
            b.interactable = true;
        }
    }

    void UpdateLevelStageText()
    {
        switch (_currentStageIndex)
        {
            case 0:
                _levelText.text = "Level 1";
                _stageText.text = "Stage 1";
                break;
            case 1:
                _levelText.text = "Level 1";
                _stageText.text = "Stage 2";
                break;
            case 2:
                _levelText.text = "Level 1";
                _stageText.text = "Stage 3";
                break;
            case 3:
                _levelText.text = "Level 2";
                _stageText.text = "Stage 1";
                break;
            case 4:
                _levelText.text = "Level 2";
                _stageText.text = "Stage 2";
                break;
            case 5:
                _levelText.text = "Level 2";
                _stageText.text = "Stage 3";
                break;
            case 6:
                _levelText.text = "Level 3";
                _stageText.text = "Stage 1";
                break;
            case 7:
                _levelText.text = "Level 3";
                _stageText.text = "Stage 2";
                break;
            case 8:
                _levelText.text = "Level 3";
                _stageText.text = "Stage 3";
                break;
        }
    }
}
