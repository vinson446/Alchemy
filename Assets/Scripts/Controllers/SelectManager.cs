using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectManager : MonoBehaviour
{
    BattleManager battleManager;

    // normal raycasts do not work on UI elements, they require a special kind
    GraphicRaycaster _raycaster;
    PointerEventData _pointerEventData;
    EventSystem _eventSystem;

    private void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
        _raycaster = GetComponent<GraphicRaycaster>();
        _eventSystem = GetComponent<EventSystem>();
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

            foreach (RaycastResult result in results)
            {
                PlayerHandSlot slot = result.gameObject.GetComponentInParent<PlayerHandSlot>();
                if (slot != null)
                {
                    int index = int.Parse(slot.name);
                    battleManager.SelectCard(index);
                    break;
                }
            }
        }
    }
}
