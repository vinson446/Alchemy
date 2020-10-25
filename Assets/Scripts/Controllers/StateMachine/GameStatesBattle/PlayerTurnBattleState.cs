using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class PlayerTurnBattleState : BattleState
{
    [SerializeField] BattleManager _battleManager;

    // normal raycasts do not work on UI elements, they require a special kind
    [SerializeField] GraphicRaycaster _raycaster;
    PointerEventData _pointerEventData;
    [SerializeField] EventSystem _eventSystem;

    [Header("Card Settings")]
    [SerializeField] float shrinkFactor;
    [SerializeField] float growthFactor;
    [SerializeField] float duration;

    public bool selectingFirstCard = true;
    public bool selectingSecondCard = false;
    public int _selectedCardIndex1 = -1;
    public int _selectedCardIndex2 = -1;

    public override void Enter()
    {
        Debug.Log("Player Turn: Entering");

        Draw();

        StateMachine.Input.PressedConfirm += OnPressedConfirm;
    }

    public override void Tick()
    {
        CheckForSelection();
    }

    public override void Exit()
    {
        Debug.Log("Player Turn: Exiting");

        StateMachine.Input.PressedConfirm -= OnPressedConfirm;
    }

    void OnPressedConfirm()
    {
        StateMachine.ChangeState<EnemyTurnBattleState>();
    }

    void Draw()
    {
        StartCoroutine(DrawCoroutine());
    }

    IEnumerator DrawCoroutine()
    {
        if (_battleManager.BattleDeck.IsEmpty)
        {
            ShuffleDeck();

            yield break;
        }

        for (int i = 0; i < _battleManager.PlayerHandPositions.Length; i++)
        {
            if (!_battleManager.BattleDeck.IsEmpty && _battleManager.PlayerHandPositions[i].transform.childCount == 0)
            {
                // front end- get top card of deck, then move card to an empty hand slot
                GameObject cardDrawn = _battleManager.DeckList[_battleManager.BattleDeck.LastIndex];
                _battleManager.PlayerHandList[i] = cardDrawn;

                cardDrawn.transform.parent = _battleManager.PlayerHandPositions[i].transform;

                CardMovement cardMovement = cardDrawn.GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    cardMovement.TargetTransform = _battleManager.PlayerHandPositions[i].transform;
                }

                _battleManager.DeckList.Remove(cardDrawn);

                yield return new WaitForSeconds(0.2f);

                // back end- draw from battle deck to player hand
                Card newCard = _battleManager.BattleDeck.Draw(DeckPosition.Top);
                _battleManager.PlayerHand.LazyAdd(newCard, i);
            }
        }
    }

    IEnumerator MoveCardsInDiscardToDeck()
    {
        int count = _battleManager.Discard.Count;
        for (int i = 0; i < count; i++)
        {
            // front end- add cards in discard to deck
            CardMovement cardMovement = _battleManager.DiscardList[0].GetComponent<CardMovement>();
            cardMovement.transform.parent = _battleManager.DeckPos;
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _battleManager.DeckPos;
            }
            _battleManager.DeckList.Add(_battleManager.DiscardList[0]);
            _battleManager.DiscardList.Remove(_battleManager.DiscardList[0]);

            // back end- add cards in discard to deck
            _battleManager.BattleDeck.Add(_battleManager.Discard.GetCard(0));
            _battleManager.Discard.Remove(0);

            yield return new WaitForSeconds(0.2f);
        }
    }

    void ShuffleDeck()
    {
        // front end
        // TODO- cool shuffle animation
        StartCoroutine(MoveCardsInDiscardToDeck());

        // back end
        _battleManager.BattleDeck.Shuffle(_battleManager.DeckList);
    }

    void CheckForSelection()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // set up new Pointer Event
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            // raycast using the graphics raycaster and mouse click position
            _raycaster.Raycast(_pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                Slot slot = result.gameObject.GetComponentInParent<Slot>();
                print(slot);
                // if player selects a card
                if (slot != null)
                {
                    int index = int.Parse(slot.name);
                    SelectCard(index, selectingFirstCard);
                }
                // if player did not select a card, reset player hand positioning
                else
                {
                    ResetSelection();

                    break;
                }
                break;
            }
        }
    }

    void ResetSelection()
    {
        // reset positioning and scale
        for (int i = 0; i < _battleManager.PlayerHandPositions.Length; i++)
        {
            CardMovement cardMovement = _battleManager.PlayerHandList[i].GetComponent<CardMovement>();
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _battleManager.PlayerHandPositions[i];
                cardMovement.transform.parent = _battleManager.PlayerHandPositions[i];
            }

            _battleManager.PlayerHandList[i].transform.DOScale(1f, 0.2f);
        }

        // reset glow images
        for (int i = 0; i < _battleManager.PlayerHandList.Count; i++)
        {
            GameObject glowImageObj = _battleManager.PlayerHandList[i];
            Image glowImage = glowImageObj.transform.GetChild(0).gameObject.GetComponent<Image>();
            glowImage.enabled = false;
        }

        // reset backend variables
        selectingFirstCard = true;
        selectingSecondCard = false;
        _selectedCardIndex1 = -1;
        _selectedCardIndex2 = -1;
    }

    // SelectManager calls this function based on card slot picked
    void SelectCard(int index, bool first)
    {
        if (first)
            _selectedCardIndex1 = index;
        else
            _selectedCardIndex2 = index;

        // shrink cards in playing hand
        ShrinkPlayerHandCards();

        // move selected cards to playing field
        MoveSelectedPlayerHandCardToField();

        // show fusion combination
        if (selectingSecondCard)
        {
            selectingSecondCard = false;

            ShowFusionCombination();
        }
        // check for fusion combinations
        else if (selectingFirstCard)
        {
            selectingFirstCard = false;
            selectingSecondCard = true;

            CheckForFusionCombinations();
        }

        // RemoveCard();
    }

    void ShrinkPlayerHandCards()
    {
        if (selectingFirstCard)
        {
            for (int i = 0; i < _battleManager.PlayerShrinkPositions.Length; i++)
            {
                CardMovement cardMovement = _battleManager.PlayerHandList[i].GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    cardMovement.TargetTransform = _battleManager.PlayerShrinkPositions[i];
                    cardMovement.transform.parent = _battleManager.PlayerShrinkPositions[i];
                }

                _battleManager.PlayerHandList[i].transform.DOScale(shrinkFactor, duration);
            }
        }
    }

    void MoveSelectedPlayerHandCardToField()
    {
        if (selectingFirstCard)
        {
            CardMovement cardMovement = _battleManager.PlayerHandList[_selectedCardIndex1].GetComponent<CardMovement>();
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _battleManager.PlayerPlayingCardPositions[0];
                cardMovement.transform.parent = _battleManager.PlayerPlayingCardPositions[0];
            }

            _battleManager.PlayerHandList[_selectedCardIndex1].transform.DOScale(growthFactor, duration);
        }
        else if (selectingSecondCard)
        {
            CardMovement cardMovement = _battleManager.PlayerHandList[_selectedCardIndex2].GetComponent<CardMovement>();
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _battleManager.PlayerPlayingCardPositions[1];
                cardMovement.transform.parent = _battleManager.PlayerPlayingCardPositions[1];
            }

            _battleManager.PlayerHandList[_selectedCardIndex2].transform.DOScale(growthFactor, duration);

            // reset glow images
            for (int i = 0; i < _battleManager.PlayerHandList.Count; i++)
            {
                GameObject glowImageObj = _battleManager.PlayerHandList[i];
                Image glowImage = glowImageObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                glowImage.enabled = false;
            }
        }
    }

    // turn on glow backgrounds of all cards in player hand that can fuse together with the currently selected card
    void CheckForFusionCombinations()
    {
        ElementCard currentCard = (ElementCard)_battleManager.PlayerHand.GetCard(_selectedCardIndex1);

        for (int i = 0; i < _battleManager.PlayerHandList.Count; i++)
        {
            if (i != _selectedCardIndex1)
            {
                ElementCard elementCard = (ElementCard)_battleManager.PlayerHand.GetCard(i);

                for (int j = 0; j < elementCard.FusionCombinations.Length; j++)
                {
                    if (currentCard.Name == elementCard.FusionCombinations[j].Name)
                    {
                        GameObject glowImageObj = _battleManager.PlayerHandList[i];
                        Image glowImage = glowImageObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                        glowImage.enabled = true;

                        break;
                    }
                }
            }
        }
    }

    void ShowFusionCombination()
    {

    }

    void RemoveCard()
    {
        // front end- remove selected card from player hand to discard
        GameObject selectedCard = _battleManager.PlayerHandList[_selectedCardIndex1];
        selectedCard.transform.parent = _battleManager.DiscardPosition;

        CardMovement cardMovement = selectedCard.GetComponent<CardMovement>();
        if (cardMovement != null)
        {
            cardMovement.TargetTransform = _battleManager.DiscardPosition;
        }

        _battleManager.PlayerHandList[_selectedCardIndex1] = null;
        _battleManager.DiscardList.Add(selectedCard);

        // back end- remove selected card from player hand to discard
        Card card = _battleManager.PlayerHand.GetCard(_selectedCardIndex1);
        _battleManager.Discard.Add(card, DeckPosition.Top);
        _battleManager.PlayerHand.LazyRemove(_selectedCardIndex1);
    }
}
