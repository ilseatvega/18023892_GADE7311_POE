using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public float speed = 0f;

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
