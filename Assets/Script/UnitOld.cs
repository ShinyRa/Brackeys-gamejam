using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitOld : MonoBehaviour
{
    Vector3 xVelocity;
    // Start is called before the first frame update
    void Start()
    {
        xVelocity = new Vector3((float)(1f * Time.deltaTime), 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.deltaTime;
        if (time == 1f)
        {
            Debug.Log(time);
        }
        transform.position += xVelocity;
    }

}
