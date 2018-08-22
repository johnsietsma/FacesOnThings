using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FaceAnimation : MonoBehaviour, ISelectHandler
{
    [SerializeField]
	[Tooltip("Animaiton speed (frames per second)")]
    private float animationSpeed = 20;

    [SerializeField]
    Image targetImage;

    [SerializeField]
    private Sprite[] sprites;

    private float animationFrameTime;
    private float startFrameTime;
    private int frameIndex;

    private Image image;
    private bool isRunning;

    void Start()
    {
        image = GetComponent<Image>();
        animationFrameTime = 1 / animationSpeed;

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

    public void OnSelect(BaseEventData data)
    {
        Debug.Log("Selected: " + gameObject.name);
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
        frameIndex = (frameIndex + 1) % sprites.Length;
        targetImage.sprite = sprites[frameIndex];
    }

}
