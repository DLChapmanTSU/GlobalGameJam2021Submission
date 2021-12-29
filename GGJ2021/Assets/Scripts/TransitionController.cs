using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField] Camera redCam;
    [SerializeField] Camera blueCam;
    [SerializeField] PlayerController player;

    //Functions responsible for swapping the players dimension (layer)
    //Also switches cameras so that only objects on the correct layer are seen

    private void Start()
    {
        redCam.enabled = true;
        blueCam.enabled = false;
    }

    public void OnRedTransition()
    {
        redCam.enabled = true;
        blueCam.enabled = false;
        player.ChangeToRed();
    }

    public void OnBlueTransition()
    {
        redCam.enabled = false;
        blueCam.enabled = true;
        player.ChangeToBlue();
    }
}
