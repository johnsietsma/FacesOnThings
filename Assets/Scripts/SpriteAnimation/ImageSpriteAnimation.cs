using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("Sprite Animation/UI Image Sprite Animation")]
[RequireComponent(typeof(SpriteAnimator))]
public class ImageSpriteAnimation : MonoBehaviour, ISpriteAnimation
{
    [SerializeField]
    Image targetImage;

    void Awake()
    {
        if( targetImage==null ) targetImage = GetComponent<Image>();
    }

    public void ShowSprite( Sprite sprite )
	{
        targetImage.sprite = sprite;
    }

}
