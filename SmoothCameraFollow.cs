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

    [SerializeField]
    private float leftStop = -Mathf.Infinity;
    [SerializeField]
    private float rightStop = Mathf.Infinity;
    // Update is called once per frame
    void FixedUpdate()
    {
        //Calculate the camera's position based on the player's position
        Vector3 movePosition = player.position + offset;

        //if player moves past leftStop, camera stops moving left
        if(player.position.x < leftStop){
            movePosition.x = leftStop;
        }
        //if player moves past rightStop, camera stops moving right
        if(player.position.x > rightStop){
            movePosition.x = rightStop;
        }

        //Horizontal Dampening
        transform.position = Vector3.SmoothDamp(transform.position,new Vector3(movePosition.x, transform.position.y,movePosition.z), ref velocity, horDamping);

        //Vertical Dampening
        transform.position = Vector3.SmoothDamp(transform.position,new Vector3(transform.position.x, movePosition.y,movePosition.z), ref velocity, vertDamping);
    }
}
