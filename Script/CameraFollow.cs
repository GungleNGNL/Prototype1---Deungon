using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]private Vector3 targetPos;
    public void NewDes(Vector3 pos)
    {
        targetPos = new Vector3(pos.x, pos.y, transform.position.z);
    }

    private void Update()
    {
        if (Vector3.Magnitude(transform.position - targetPos) > 0.001f)
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 12.0f * Time.deltaTime);
    }
}
