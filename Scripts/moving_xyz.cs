using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class moving_xyz : MonoBehaviour
{

    // Speed in units per sec.
    public float speed;
    public float rotateSpeed;
    public float degree;
    public float target_x,target_y,target_z;
    private List<Vector3> targetList;
    private int current = 1;
    
    
    private void OnEnable()
    {
        targetList = new List<Vector3>();
        targetList.Add(new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z));
        targetList.Add(new Vector3(transform.localPosition.x+target_x, transform.localPosition.y+target_y, transform.localPosition.z+target_z));
    }


    void Update()
    {
        if (speed != 0)
        {
            Move();
        }
        if(rotateSpeed != 0)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * rotateSpeed * degree * Time.deltaTime);
    }

    private void Move()
    {
        float step = speed * Time.deltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetList[current], step);
        if (Vector3.Distance(targetList[current], transform.localPosition) < 0.001f)
        {
            if (current == 0)
            {
                current = 1;
            }
            else
            {
                current = 0;
            }
        }
    }
}
