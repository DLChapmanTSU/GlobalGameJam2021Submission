using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Sprite closed;
    [SerializeField] Sprite open;

    //Functions purely handle swapping between sprites and enabling/disabling the collider for the door

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = closed;
    }

    public void Open()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = open;
    }

    public void Close()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = closed;
    }
}
