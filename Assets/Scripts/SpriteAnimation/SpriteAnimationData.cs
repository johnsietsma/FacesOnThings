using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SpriteAnimationData : ScriptableObject
{
    [Tooltip("Animaiton speed (frames per second)")]
    public float animationSpeed = 20;

    public Sprite[] sprites;

}
