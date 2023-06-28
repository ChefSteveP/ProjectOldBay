using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float horDamping;
    public float vertDamping;
    private Vector3 velocity = Vector3.zero;
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movePosition = player.position + offset;

        //Horizontal Dampening
        transform.position = Vector3.SmoothDamp(transform.position,new Vector3(movePosition.x, transform.position.y,movePosition.z), ref velocity, horDamping);

        //Vertical Dampening
        transform.position = Vector3.SmoothDamp(transform.position,new Vector3(transform.position.x, movePosition.y,movePosition.z), ref velocity, vertDamping);
    }
}
