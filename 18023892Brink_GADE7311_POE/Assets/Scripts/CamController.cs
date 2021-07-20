using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform camZero;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(offset.x, offset.y, camZero.position.z);
    }
}
