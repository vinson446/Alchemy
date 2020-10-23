using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckSelectManager : MonoBehaviour
{
    // normal raycasts do not work on UI elements, they require a special kind
    GraphicRaycaster _raycaster;
    PointerEventData _pointerEventData;
    EventSystem _eventSystem;

    [SerializeField] ElementCardView selectedCard;

    LevelSelectSM StateMachine;
    ShowUpgradeState upgradeState;

    private void Awake()
    {
        _raycaster = GetComponent<GraphicRaycaster>();
        _eventSystem = GetComponent<EventSystem>();

        StateMachine = FindObjectOfType<LevelSelectSM>();
        upgradeState = FindObjectOfType<ShowUpgradeState>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // set up new Pointer Event
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            // raycast using the graphics raycaster and mouse click position
            _raycaster.Raycast(_pointerEventData, results);

            // if a card is selected -> takes player to upgrade panel
            if (selectedCard == null)
            {
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.GetComponentInParent<ElementCardView>() != null)
                    {
                        Slot slot = result.gameObject.GetComponentInParent<Slot>();
                        if (slot != null)
                        {
                            int index = int.Parse(slot.name);

                            selectedCard = result.gameObject.GetComponentInParent<ElementCardView>();
                            upgradeState.CardInDeckIndex = index;

                            StateMachine.Input.DetectViewUpgrade();
                        }
                        break;
                    }
                }
            }
            // upgrade panel = compare next selected card with selected card on screen
            // if it's not the same card, go back to deck panel
            else
            {
                bool touchedSelectedCard = false;

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.GetComponentInParent<SelectedCard>() != null || result.gameObject.name == "Upgrade Button")
                    {
                        if (result.gameObject.name == "Upgrade Button")
                            selectedCard = null;
                        else
                            touchedSelectedCard = true;
                        break;
                    }
                }

                // if we have a selected card and we click off it, go back to deck panel
                if (!touchedSelectedCard && selectedCard != null)
                {
                    selectedCard = null;

                    StateMachine.Input.DetectViewDeck();
                }
            }
        }
    }
}
