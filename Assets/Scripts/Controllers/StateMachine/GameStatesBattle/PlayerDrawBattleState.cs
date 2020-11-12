using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerDrawBattleState : BattleState
{
    [SerializeField] BattleManager _battleManager;

    PlayerTurnBattleState playerTurnBattleState;

    SoundEffects soundEffects;

    private void Start()
    {

    }

    public override void Enter()
    {
        playerTurnBattleState = GetComponent<PlayerTurnBattleState>();
        soundEffects = FindObjectOfType<SoundEffects>();

        Draw();

        playerTurnBattleState.ResetFusionMonsterPositioning();
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override void Exit()
    {
        base.Exit();
    }

    void Draw()
    {
        StartCoroutine(DrawCoroutine());
    }

    IEnumerator DrawCoroutine()
    {
        for (int i = 0; i < _battleManager.PlayerHandPositions.Length; i++)
        {
            if (_battleManager.BattleDeck.IsEmpty)
            {
                ShuffleDeck();

                yield return new WaitForSeconds(2f);
            }

            if (!_battleManager.BattleDeck.IsEmpty && _battleManager.PlayerHandPositions[i].transform.childCount == 0)
            {
                // front end- get top card of deck, then move card to an empty hand slot
                GameObject cardDrawn = _battleManager.DeckList[_battleManager.BattleDeck.LastIndex];
                _battleManager.PlayerHandList[i] = cardDrawn;

                cardDrawn.transform.SetParent(_battleManager.PlayerHandPositions[i].transform);

                CardMovement cardMovement = cardDrawn.GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    cardMovement.TargetTransform = _battleManager.PlayerHandPositions[i].transform;
                }

                _battleManager.DeckList.Remove(cardDrawn);

                soundEffects.PlayCardSound();

                yield return new WaitForSeconds(0.2f);

                // back end- draw from battle deck to player hand
                Card newCard = _battleManager.BattleDeck.Draw(DeckPosition.Top);
                _battleManager.PlayerHand.LazyAdd(newCard, i);
            }
        }

        playerTurnBattleState.ResetBothEnds();
        StateMachine.ChangeState<PlayerTurnBattleState>();
    }

    IEnumerator MoveCardsInDiscardToDeck()
    {
        int count = _battleManager.Discard.Count;
        for (int i = 0; i < count; i++)
        {
            soundEffects.PlayCardSound();

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

            yield return new WaitForSeconds(0.1f);
        }

        // back end
        _battleManager.BattleDeck.Shuffle(_battleManager.DeckList);
        ShowAllCardsInDeck();
    }

    void ShuffleDeck()
    {
        // front end
        StartCoroutine(MoveCardsInDiscardToDeck());
    }

    // display cards visually in battle deck
    void ShowAllCardsInDeck()
    {
        for (int i = 0; i < _battleManager.BattleDeck.Count; i++)
        {
            ElementCardView c = _battleManager.DeckList[i].GetComponent<ElementCardView>();
            ElementCard newCard = (ElementCard)_battleManager.BattleDeck.GetCard(i);
            c.Display(newCard);
        }
    }
}
