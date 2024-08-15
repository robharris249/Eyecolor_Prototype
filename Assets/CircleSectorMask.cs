using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSectorMask : MonoBehaviour {

    public Image maskImage;
    public float sectorAngle = 90f; // Default angle of 90 degrees
    private RectTransform maskRect;

    private void Start() {
        maskRect = maskImage.rectTransform;
    }

    private void Update() {
        // Ensure the sector angle stays within the valid range (0 to 180 degrees)
        sectorAngle = Mathf.Clamp(sectorAngle, 0f, 180f);

        // Update the mask's position and rotation
        float radius = Mathf.Max(maskRect.rect.width, maskRect.rect.height) * 0.5f;
        float angle = sectorAngle * 0.5f;
        float radians = angle * Mathf.Deg2Rad;
        float x = radius * Mathf.Sin(radians);
        float y = radius * Mathf.Cos(radians);

        maskRect.anchoredPosition = new Vector2(x, y);
        maskRect.localRotation = Quaternion.Euler(0f, 0f, -angle);

        // Update the mask's size to fill the sector
        float sizeMultiplier = 2f * Mathf.Tan(radians);
        maskRect.localScale = new Vector3(sizeMultiplier, sizeMultiplier, 1f);
    }
}