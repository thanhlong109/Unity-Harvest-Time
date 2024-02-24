using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FarmerShadow : MonoBehaviour
{
    [SerializeField] private Transform farmerTransform;
    [SerializeField] private Vector2 shadowOffset = new Vector2(0.302f, 0.335f);
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Light2D light2d;

    void Start()
    {
        light2d = GetComponent<Light2D>();
    }

    
    void Update()
    {
        transform.position = new Vector3(farmerTransform.position.x + shadowOffset.x, farmerTransform.position.y+ shadowOffset.y, farmerTransform.position.z);
        light2d.lightCookieSprite = spriteRenderer.sprite;
    }
}
