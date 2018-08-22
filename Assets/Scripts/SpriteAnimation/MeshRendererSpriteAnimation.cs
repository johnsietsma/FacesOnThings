using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[AddComponentMenu("Sprite Animation/MeshRenderer Sprite Animation")]
[RequireComponent(typeof(MeshRenderer), typeof(SpriteAnimator))]
public class MeshRendererSpriteAnimation : MonoBehaviour, ISpriteAnimation
{
    private Material flipBookMaterial;

    void Awake()
    {
        flipBookMaterial = GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void ShowSprite(Sprite sprite)
    {
        Rect rect = sprite.rect;
        float width = sprite.texture.width;
        float height = sprite.texture.height;

        flipBookMaterial.mainTexture = sprite.texture;
        flipBookMaterial.mainTextureOffset = new Vector2(rect.x / width, rect.y / height);
        flipBookMaterial.mainTextureScale = new Vector2(rect.width / width, rect.height / height);
    }
}
