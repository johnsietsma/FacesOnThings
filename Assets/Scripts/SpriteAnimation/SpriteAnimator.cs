using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteAnimationData AnimationData
    {
        get { return animationData; }
        set
        {
            animationData = value;
            // Restart to refresh frame and frame index data
            if( animationData!=null ) StartAnimation();
        }
    }

    [SerializeField]
    private SpriteAnimationData animationData;

    ISpriteAnimation spriteAnimation;

    private float animationFrameTime;
    private float startFrameTime;
    private int frameIndex;

    private bool isRunning;

    void Start()
    {
        spriteAnimation = GetComponent<ISpriteAnimation>();
        if (spriteAnimation==null) Debug.LogError("GameObject has a SpriteAnimator, but not an SpriteAnimation Component. Add a UIImageSpriteAnimttion or a MeshRendererSpriteAnimation.");
        if( AnimationData!=null ) StartAnimation();
    }

    void Update()
    {
        if (!isRunning) return;

        if (Time.time > startFrameTime + animationFrameTime)
        {
            startFrameTime = Time.time;
            ShowNextFrame();
        }
    }

    public void StartAnimation()
    {
        startFrameTime = Time.time;
        isRunning = true;
        frameIndex = 0;
        animationFrameTime = 1 / animationData.animationSpeed;
    }

    public void StopAnimation()
    {
        startFrameTime = 0;
        isRunning = false;
    }

    private void ShowNextFrame()
    {
        frameIndex = (frameIndex + 1) % animationData.sprites.Length;
        spriteAnimation.ShowSprite(animationData.sprites[frameIndex]);
    }

}
