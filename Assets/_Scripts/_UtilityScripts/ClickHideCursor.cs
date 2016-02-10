using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickHideCursor : MonoBehaviour
{

    public bool isLocked = false; // Wether cursor is locked or not (paused or not)

    void Start()
    {
        //setCursorLock(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Locking system
    void setCursorLock(bool isLocked)
    {
        this.isLocked = isLocked;
        Cursor.visible = !isLocked; //Set cursor visible or invisible

        //If games is not paused
        if (isLocked == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Time.timeScale = 1;
        }
        else if (isLocked == false)
        {
            Cursor.lockState = CursorLockMode.None;
            //Time.timeScale = 0; //Pause all the game (except for camera)
            //Add a way to stop the camera too
        }
    }

    void Update()
    {
        /*
        if (!isLocked && (Input.GetMouseButtonDown(0)))
        {
            setCursorLock(!isLocked);
        }

        // When player press escape
        if (Input.GetKeyDown(KeyCode.Escape) && isLocked)
        {
            //reverse variable isLocked
            //if variable isLocked is true, then it goes to false
            //If true, then cursor is locked, if false cursor is unlocked (may be the other way but I don't want to test it )
            setCursorLock(!isLocked);
        }
        */
    }
}
