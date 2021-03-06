﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteAnimator))]
public class AnimationDataCopySelectHandler : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private SpriteAnimator targetSpriteAnimator;

    private SpriteAnimator spriteAnimator;

    void Awake()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();
    }

    public void OnSelect(BaseEventData data)
    {
        Debug.Log("Copy data: " + spriteAnimator.AnimationData.name);
        targetSpriteAnimator.AnimationData = spriteAnimator.AnimationData;
    }
}
