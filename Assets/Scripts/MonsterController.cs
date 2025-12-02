using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    float speed = 2f;


    private void FixedUpdate()
    {
        transform.position += new Vector3(- speed * Time.deltaTime, 0, 0);
    }
}
