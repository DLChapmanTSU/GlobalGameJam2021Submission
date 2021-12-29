using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Vector2 movement;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    bool isRed = true;
    bool isGrounded;
    bool isHolding = false;
    bool isFacingRight = true;
    bool canSwitch = false;
    bool transitioning = false;
    bool switchingScenes = false;

    [SerializeField] Transform playerFeet;
    [SerializeField] Transform playerHands;
    [SerializeField] LayerMask redGround;
    [SerializeField] LayerMask blueGround;
    [SerializeField] LayerMask dropMask;

    GameObject objectToPickup;
    GameObject heldObject;

    [SerializeField] Sprite redBoxSprite;
    [SerializeField] Sprite blueBoxSprite;

    [SerializeField] GameObject wipeObj;
    Animator wipeAnim;

    [SerializeField] GameObject fadeObj;
    Animator transitionAnimator;
    SceneFade sceneFade;

    [SerializeField] AudioSource jumpSource;
    [SerializeField] AudioSource shiftSource;
    [SerializeField] AudioSource interactSource;
    [SerializeField] AudioSource switcherSource;

    // Start is called before the first frame update
    void Start()
    {
        wipeAnim = wipeObj.GetComponent<Animator>();
        transitionAnimator = fadeObj.GetComponent<Animator>();
        sceneFade = fadeObj.GetComponent<SceneFade>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Move();

        if (isRed == true)
        {
            isGrounded = Physics2D.OverlapCircle(playerFeet.position, 0.1f, redGround);
        }
        else
        {
            isGrounded = Physics2D.OverlapCircle(playerFeet.position, 0.1f, blueGround);
        }
    }

    //Moves player based on horizontal input which is saved in "movement"
    void Move()
    {
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
        if (movement.x > 0 && isFacingRight == false)
        {
            Vector3 flipScale = transform.localScale;
            flipScale.x *= -1;
            transform.localScale = flipScale;
            isFacingRight = true;
        }
        else if (movement.x < 0 && isFacingRight == true)
        {
            Vector3 flipScale = transform.localScale;
            flipScale.x *= -1;
            transform.localScale = flipScale;
            isFacingRight = false;
        }
    }

    //Will trigger the transition animation to flip the player to the opposite dimension
    void OnSwitchView()
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }

        if (transitioning == true)
        {
            return;
        }

        if (switchingScenes == true)
        {
            return;
        }

        if (heldObject != null)
        {
            if (heldObject.layer == 14 || heldObject.layer == 15)
            {
                return;
            }
        }

        if (isRed == true)
        {
            transitioning = true;
            shiftSource.Play();
            wipeAnim.SetTrigger("BlueSwitch");
        }
        else if (isRed == false)
        {
            transitioning = true;
            shiftSource.Play();
            wipeAnim.SetTrigger("RedSwitch");
        }
    }

    public void ChangeToRed()
    {
        gameObject.layer = 8;
        isRed = true;
        transitioning = false;
    }

    public void ChangeToBlue()
    {
        gameObject.layer = 9;
        isRed = false;
        transitioning = false;
    }

    public void SetSwitching(bool x)
    {
        switchingScenes = x;
    }

    //Applies an upward force to the player to simulate a jump
    void OnJump()
    {
        if (isGrounded == true)
        {
            rb.AddForce(Vector2.up * jumpForce);
            jumpSource.Play();
        }
    }

    //Drops the current object held if an object is held or picks up a new object if hands are empty
    //Will not work if the game is paused or if the game is transitioning between scenes
    //If an item is held and a portal is in range, the item is flipped to the opposite dimension and then dropped
    void OnInteract()
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }

        if (transitioning == true)
        {
            return;
        }

        if (switchingScenes == true)
        {
            return;
        }

        if (objectToPickup != null && isHolding == false)
        {
            objectToPickup.transform.parent = playerHands;
            heldObject = objectToPickup;
            heldObject.GetComponent<Rigidbody2D>().isKinematic = true;
            heldObject.GetComponent<BoxCollider2D>().enabled = false;
            heldObject.transform.position = playerHands.position;
            heldObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isHolding = true;
            interactSource.Play();
        }
        else if (isHolding == true && canSwitch == false)
        {
            if (Physics2D.OverlapCircle(playerHands.position, 0.1f, dropMask) == false)
            {
                heldObject.transform.parent = null;
                heldObject.GetComponent<Rigidbody2D>().isKinematic = false;
                heldObject.GetComponent<BoxCollider2D>().enabled = true;
                heldObject = null;
                isHolding = false;
            }
        }
        else if (isHolding == true && canSwitch == true)
        {
            heldObject.transform.parent = null;
            heldObject.GetComponent<Rigidbody2D>().isKinematic = false;
            heldObject.GetComponent<BoxCollider2D>().enabled = true;
            if (heldObject.layer == 14)
            {
                heldObject.GetComponent<SpriteRenderer>().sprite = blueBoxSprite;
                heldObject.layer = 15;
            }
            else if (heldObject.layer == 15)
            {
                heldObject.GetComponent<SpriteRenderer>().sprite = redBoxSprite;
                heldObject.layer = 14;
            }

            heldObject = null;
            isHolding = false;

            switcherSource.Play();
        }
    }

    //Triggers scene transition to reload the current scene
    void OnRestart()
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }

        if (transitioning == true)
        {
            return;
        }

        if (switchingScenes == true)
        {
            return;
        }
        Debug.Log("Restart");

        switchingScenes = true;
        sceneFade.SetIndex(SceneManager.GetActiveScene().buildIndex);
        transitionAnimator.SetTrigger("Fade");
    }

    //Triggers scene transition to load the main menu
    void OnQuit()
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }

        if (transitioning == true)
        {
            return;
        }

        if (switchingScenes == true)
        {
            return;
        }
        Debug.Log("Quit");

        switchingScenes = true;
        sceneFade.SetIndex(0);
        transitionAnimator.SetTrigger("Fade");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pickup" && isHolding == false)
        {
            //Sets a reference to the object that is in the trigger for picking up objects
            objectToPickup = collision.gameObject;
            Debug.Log("Item To Pickup");
        }
        else if (collision.gameObject.tag == "Switcher")
        {
            canSwitch = true;
            Debug.Log("In Switcher");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pickup")
        {
            //Removes the reference to the object that would be picked up
            objectToPickup = null;
            Debug.Log("Item Left");
        }
        else if (collision.gameObject.tag == "Switcher")
        {
            canSwitch = false;
            Debug.Log("Out Switcher");
        }
    }
}
