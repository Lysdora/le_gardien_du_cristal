using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Target")]
    private Transform playerTransform;

    [Header("Limit")]
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 40f;
    [SerializeField] private float minY = -5f;
    [SerializeField] private float maxY = 5f;

    private Vector3 tempPos;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;

        if (playerTransform == null)
            Debug.Log("Ajoute le tag player � ton player !");
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        tempPos = new Vector3(
            playerTransform.position.x,
             playerTransform.position.y,
            transform.position.z
        );

        tempPos.x = Mathf.Clamp( tempPos.x, minX, maxX );
        tempPos.y = Mathf.Clamp( tempPos.y, minY, maxY );

        transform.position = tempPos;

        

    }


}
