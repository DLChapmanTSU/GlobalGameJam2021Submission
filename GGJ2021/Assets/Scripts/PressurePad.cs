using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    [SerializeField] Door door;
    [SerializeField] Sprite pressedButtonSprite;
    private Sprite startSprite;
    [SerializeField] AudioSource onSource;
    [SerializeField] AudioSource offSource;

    private void Start()
    {
        startSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pickup")
        {
            //Opens corresponding door when a box is placed on the button
            Debug.Log("Triggered");
            GetComponent<SpriteRenderer>().sprite = pressedButtonSprite;
            onSource.Play();
            door.Open();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pickup")
        {
            //Closes the door when the box is removed from the button
            Debug.Log("Untriggered");
            GetComponent<SpriteRenderer>().sprite = startSprite;
            offSource.Play();
            door.Close();
        }
    }
}
