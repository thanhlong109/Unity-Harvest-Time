using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CanvasSlide : MonoBehaviour
{
    public RectTransform canvasContainer; 
    public Vector2 startPosition;         
    public Vector2 endPosition;          
    public float duration = 1.0f;          

    private bool isMoving = false;

    private void Awake()
    {
        canvasContainer = GetComponent<RectTransform>();
    }

    void Start()
    {
        
        canvasContainer.anchoredPosition = startPosition;
    }

    public void SlideIn()
    {
        if (!isMoving)
            StartCoroutine(MoveToPosition(endPosition));
    }

    public void SlideOut()
    {
        if (!isMoving)
            StartCoroutine(MoveToPosition(startPosition));
    }

    IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        isMoving = true;
        float time = 0.0f;
        Vector2 startPosition = canvasContainer.anchoredPosition;

        while (time < duration)
        {
            canvasContainer.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasContainer.anchoredPosition = targetPosition;
        isMoving = false;
    }
}
