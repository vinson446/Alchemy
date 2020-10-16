using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { Element, Spell };

public abstract class Card 
{
    public CardType cardType { get; protected set; }

    public string Name { get; protected set; } = "...";
    public int Level { get; protected set; } = 1;
    public string Description { get; protected set; }

    public Sprite Sprite { get; protected set; }
    public Sprite SpriteBackground { get; protected set; }
    public Sprite CardBackground { get; protected set; }

    public CardPlayEffect PlayEffect { get; protected set; }

    public abstract void Play();
}
