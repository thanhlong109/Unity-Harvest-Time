using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMouseDown : MonoBehaviour
{


    void Update()
    {
       if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                var gameObj = hit.collider.gameObject;
                if (gameObj.CompareTag("Plot"))
                {
                    gameObj.SendMessage("BeingMouseClicked");
                }
                Debug.Log(gameObj.name);
            }
        }
    }
}
