using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UI_Limb : MonoBehaviour
{
    public Image image;
    public RectTransform rect;
    private Sprite lastSprite;

    public Limb limbForThis;

    [MyBox.ButtonMethod()]
    private void Awake()
    {
        if (limbForThis == null)
            return;
        image = GetComponent<Image>();
        image.sprite = limbForThis.spriteForUi;
        lastSprite = image == null ? null : image.sprite;
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (limbForThis == null)
            return;
        if (image != null && lastSprite != limbForThis.spriteForUi)
        {
            image.sprite = limbForThis.spriteForUi;
            lastSprite = limbForThis.spriteForUi;

            float y = rect.sizeDelta.x * image.sprite.bounds.extents.y / image.sprite.bounds.extents.x;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, y);
        }
    }
}
