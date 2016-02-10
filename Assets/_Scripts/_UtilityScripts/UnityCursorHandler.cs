using UnityEngine;
using System.Collections;

public class UnityCursorHandler: MonoBehaviour {

    public static bool locked = true;
    
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        if(locked)
        {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        }

        if(!locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            locked = false;

        if (Input.GetMouseButtonDown(0))
            locked = true;
    }

    /*
    // Apply requested cursor state
    void SetCursorState()
    {
        Cursor.lockState = wantedMode;
		// Hide cursor when locking
		Cursor.visible = (CursorLockMode.Locked != wantedMode);
	}
	
	void OnGUI ()
	{
        GUILayout.BeginHorizontal();
		// Release cursor on escape keypress
		if (Input.GetKeyDown (KeyCode.Escape))
			Cursor.lockState = wantedMode = CursorLockMode.None;
			
		switch (Cursor.lockState)
		{
			case CursorLockMode.None:
				GUILayout.Label ("Cursor is normal");
				if (GUILayout.Button ("Lock cursor"))
					wantedMode = CursorLockMode.Locked;
				if (GUILayout.Button ("Confine cursor"))
					wantedMode = CursorLockMode.Confined;
				break;
			case CursorLockMode.Confined:
				GUILayout.Label ("Cursor is confined");
				if (GUILayout.Button ("Lock cursor"))
					wantedMode = CursorLockMode.Locked;
				if (GUILayout.Button ("Release cursor"))
					wantedMode = CursorLockMode.None;
				break;
			case CursorLockMode.Locked:
				GUILayout.Label ("Cursor is locked");
				if (GUILayout.Button ("Unlock cursor"))
					wantedMode = CursorLockMode.None;
				if (GUILayout.Button ("Confine cursor"))
					wantedMode = CursorLockMode.Confined;
				break;
		}

        GUILayout.EndVertical();

		SetCursorState ();
	}
    */
}
