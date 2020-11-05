using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// STRETCH public enum CardType { Element, Spell };

public abstract class Card 
{
    // STRETCH public CardType cardType { get; protected set; }

    public string Name { get; protected set; } = "...";
    public int Level { get; protected set; } = 1;
    public string Description { get; protected set; }

    public Sprite Sprite { get; protected set; }
    public Sprite SpriteBackground { get; protected set; }
    public Sprite CardBackground { get; protected set; }

    public CardPlayEffect CardEffect { get; protected set; }

    public abstract void Play();
}
