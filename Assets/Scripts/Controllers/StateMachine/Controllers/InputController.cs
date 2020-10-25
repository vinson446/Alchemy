using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    // general input
    public event Action PressedGoToMenu = delegate { };
    public event Action PressedGoToLevelSelect = delegate { };
    public event Action PressedGoToBattle = delegate { };

    // level select input
    // public event Action PressedViewLab = delegate { };
    public event Action PressedViewDeck = delegate { };
    public event Action PressedViewUpgrade = delegate { };
    public event Action PressedRight = delegate { };
    public event Action PressedLeft = delegate { };

    // battle input
    public event Action PressedConfirm = delegate { };

    private void Update()
    {

    }

    // general input
    public void DetectMenu()
    {
        PressedGoToMenu?.Invoke();
    }

    public void DetectLevelSelect()
    {
        PressedGoToLevelSelect?.Invoke();
    }

    public void DetectBattle()
    {
        PressedGoToBattle?.Invoke();
    }

    // level select- quit
    /*
    public void DetectViewLab()
    {
        PressedViewLab?.Invoke();
    }
    */

    public void DetectViewDeck()
    {
        PressedViewDeck?.Invoke();
    }

    public void DetectViewUpgrade()
    {
        PressedViewUpgrade?.Invoke();
    }

    public void DetectRight()
    {
        PressedRight?.Invoke();
    }

    public void DetectLeft()
    {
        PressedLeft?.Invoke();
    }

    // battle input
    public void DetectConfirm()
    {
        PressedConfirm?.Invoke();
    }
}
