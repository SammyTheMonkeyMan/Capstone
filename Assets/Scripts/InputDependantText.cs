using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputDependantText : MonoBehaviour
{
    // refs to Button's components
    //private Image buttonImage;

    // refs to your sprites
    //public Sprite gamepadImage;
    //public Sprite keyboardImage;

    void Awake()
    {
        //buttonImage = GetComponent<Image>();
        PlayerInput input = FindObjectOfType<PlayerInput>();
        updateButtonImage(input.currentControlScheme);
    }

    void OnEnable()
    {
        InputUser.onChange += onInputDeviceChange;
    }

    void OnDisable()
    {
        InputUser.onChange -= onInputDeviceChange;
    }

    void onInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change == InputUserChange.ControlSchemeChanged)
        {
            updateButtonImage(user.controlScheme.Value.name);
        }
    }

    void updateButtonImage(string schemeName)
    {
        // assuming you have only 2 schemes: keyboard and gamepad
        if (schemeName.Equals("Gamepad"))
        {
            //buttonImage.sprite = gamepadImage;
        }
        else
        {
            //buttonImage.sprite = keyboardImage;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
