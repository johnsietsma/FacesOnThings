using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
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

        animationFrameTime = 1 / animationData.animationSpeed;
        StartAnimation();
    }

	void Update()
	{
        if( !isRunning ) return;

        if( Time.time > startFrameTime + animationFrameTime )
		{
            startFrameTime = Time.time;
            ShowNextFrame();
        }
	}

    public void StartAnimation()
	{
        startFrameTime = Time.time;
        isRunning = true;
    }

	public void StopAnimation()
	{
        startFrameTime = 0;
        isRunning = false;
    }

	private void ShowNextFrame()
	{
        frameIndex = (frameIndex + 1) % animationData.sprites.Length;
        spriteAnimation.ShowSprite( animationData.sprites[frameIndex] );
    }

}
