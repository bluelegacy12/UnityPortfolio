using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        speed = ThirdPersonMovement.thirdPersonMovement.speed;
        
        /* if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            transform.position = touchPosition;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Camera.main.ScreenToWorldPoint(Input.touch[i].position);
                Debug.DrawLine(Vector3.zero, touchPosition, Color.blue);
            }
        } */
    }
}
