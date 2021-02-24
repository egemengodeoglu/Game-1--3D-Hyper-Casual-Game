using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public float obstacleSpeed;
    public GameObject obj1,obj2,obj3,obj4;
    public Transform target1, target2;
    private Vector3 origin1, origin2;

    private void Start()
    {
        origin1 = obj1.transform.position;
        origin2 = obj2.transform.position;
    }
    void Update()
    {
        float step = obstacleSpeed * Time.deltaTime;
        obj1.transform.position = Vector3.MoveTowards(obj1.transform.position, target1.position, step);
        obj2.transform.position = Vector3.MoveTowards(obj2.transform.position, target2.position, step);
        obj3.transform.position = Vector3.MoveTowards(obj3.transform.position, target1.position, step);
        obj4.transform.position = Vector3.MoveTowards(obj4.transform.position, target2.position, step);

        if (Vector3.Distance(obj1.transform.position, target1.position) < 0.001f)
        {
            obj1.transform.position = origin1;
            obj2.transform.position = origin2;
        }else if(Vector3.Distance(obj3.transform.position, target1.position) < 0.001f)
        {
            obj3.transform.position = origin1;
            obj4.transform.position = origin2;
        }
    }
}
