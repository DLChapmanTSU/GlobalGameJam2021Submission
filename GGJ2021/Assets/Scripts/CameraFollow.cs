using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void Awake()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Performs a linear interpolation to allow the camera to follow the player smoothly
        Vector3 current = new Vector3(transform.position.x, transform.position.y, -10.0f);
        Vector3 destination = new Vector3(player.transform.position.x, player.transform.position.y, -10.0f);
        transform.position = Vector3.Lerp(current, destination, Time.deltaTime);
    }
}
