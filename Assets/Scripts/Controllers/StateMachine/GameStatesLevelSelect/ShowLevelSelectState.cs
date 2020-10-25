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
    [SerializeField] GameObject _levelSelectPanel;

    [SerializeField] GameObject[] _allLevels;
    [SerializeField] Button[] _sideButtons;
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] TextMeshProUGUI _stageText;

    int _currentStageIndex;
    int _previousStageIndex;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();    
    }

    public override void Enter()
    {
        _levelSelectPanel.SetActive(true);

        // StateMachine.Input.PressedViewLab += OnPressedLab;
        StateMachine.Input.PressedViewDeck += OnPressedDeck;

        StateMachine.Input.PressedGoToMenu += OnPressedMenu;
        StateMachine.Input.PressedGoToBattle += OnPressedBattle;

        StateMachine.Input.PressedRight += RightArrow;
        StateMachine.Input.PressedLeft += LeftArrow;
    }

    public override void Tick()
    {

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

        StateMachine.Input.PressedRight -= RightArrow;
        StateMachine.Input.PressedLeft -= LeftArrow;
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
        SceneManager.LoadScene("Battle");
    }

    void OnPressedMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void RightArrow()
    {
        if ((_currentStageIndex < _allLevels.Length - 1) && (_currentStageIndex < gameManager.LevelsUnlocked))
        {
            _previousStageIndex = _currentStageIndex;
            _currentStageIndex += 1;

            for (int i = _allLevels.Length - 1; i > 0; i--)
            {
                _allLevels[i].transform.DOMoveX(_allLevels[i - 1].transform.position.x, 0.25f, true);
            }
            _allLevels[0].transform.DOMoveX(_allLevels[0].transform.position.x - 750, 0.25f, true);

            MinimizePreviousLevel();
            EnlargeCurrentLevel();

            StartCoroutine(DisableRightLeft());

            UpdateLevelStageText();
        }
    }

    void LeftArrow()
    {
        if ((_currentStageIndex > 0))
        {
            _previousStageIndex = _currentStageIndex;
            _currentStageIndex -= 1;

            for (int i = 0; i < _allLevels.Length - 1; i++)
            {
                _allLevels[i].transform.DOMoveX(_allLevels[i + 1].transform.position.x, 0.25f, true);
            }
            _allLevels[_allLevels.Length - 1].transform.DOMoveX(_allLevels[_allLevels.Length - 1].transform.position.x + 750, 0.25f, true);

            MinimizePreviousLevel();
            EnlargeCurrentLevel();

            StartCoroutine(DisableRightLeft());

            UpdateLevelStageText();
        }
    }

    void MinimizePreviousLevel()
    {
        _allLevels[_previousStageIndex].transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400);
        _allLevels[_previousStageIndex].transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600);
    }

    void EnlargeCurrentLevel()
    {
        _allLevels[_currentStageIndex].transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 600);
        _allLevels[_currentStageIndex].transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 800);
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
