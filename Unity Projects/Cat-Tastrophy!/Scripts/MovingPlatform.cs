using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject Player;
    float timeValue;
    bool forwardTrigger;
    bool backwardTrigger;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        forwardTrigger = true;
        backwardTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > startPosition.z - 10 && forwardTrigger)
        {
            transform.Translate(transform.forward * -1 * Time.deltaTime);
        }
        if (transform.position.z <= startPosition.z - 10)
        {
            forwardTrigger = false;
            backwardTrigger = true;
        }
        if (transform.position.z < startPosition.z && backwardTrigger)
        {
            transform.Translate(transform.forward * 1 * Time.deltaTime);
        }
        if (transform.position.z >= startPosition.z)
        {
            backwardTrigger = false;
            forwardTrigger = true;
        }
    }
}
