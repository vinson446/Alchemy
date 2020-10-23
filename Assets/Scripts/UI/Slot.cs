using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] int _slotNum;
    public int SlotNum { get => _slotNum; private set => _slotNum = value; }
}
