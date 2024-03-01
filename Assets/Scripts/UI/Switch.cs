using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    [SerializeField] private Image onSprite;
    [SerializeField] private Image offSprite;
    [SerializeField] private RectTransform onDisplay;
    [SerializeField] private RectTransform offDisplay;

    private void Start()
    {
        onSprite.AddComponent<Button>().onClick.AddListener(Off) ;
        offSprite.AddComponent<Button>().onClick.AddListener(On);
        On();
    }

    public void On()
    {
        onSprite.gameObject.SetActive(true);
        offSprite.gameObject.SetActive(false);
        onDisplay.gameObject.SetActive(true);
        offDisplay.gameObject.SetActive(false);
    }

    public void Off()
    {
        onSprite.gameObject.SetActive(false) ;
        offSprite.gameObject.SetActive(true) ;
        offDisplay.gameObject.SetActive(true) ;
        onDisplay.gameObject.SetActive(false) ;
    }
}
