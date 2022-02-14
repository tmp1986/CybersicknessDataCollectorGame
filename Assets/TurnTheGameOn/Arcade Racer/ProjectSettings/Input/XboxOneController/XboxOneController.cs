using UnityEngine;
using UnityEngine.UI;

public class XboxOneController : MonoBehaviour
{
    public Image leftStick;
    public Image rightStick;
    public Image leftTrigger;
    public Image rightTrigger;
    public Image leftBumper;
    public Image rightBumper;
    public Image aButton;
    public Image bButton;
    public Image xButton;
    public Image yButton;
    public Image dPadLeft;
    public Image dPadUp;
    public Image dPadRight;
    public Image dPadDown;
    public Image startButton;
    public Image backButton;

    private void Update()
    {
        #region Joysticks
        float moveHL = Input.GetAxis("Left Joystick Horizontal") * 100;
        float moveVL = Input.GetAxis("Left Joystick Vertical") * 100;
        leftStick.rectTransform.localPosition = new Vector2(moveHL, moveVL);

        float moveHR = Input.GetAxis("Right Joystick Horizontal") * 100;
        float moveVR = Input.GetAxis("Right Joystick Vertical") * 100;
        rightStick.rectTransform.localPosition = new Vector2(moveHR, moveVR);
        #endregion


        #region Joystick Buttons
        bool leftJoystickIsPressed = Input.GetButton("Left Joystick Button");
        leftStick.color = leftJoystickIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);

        bool rightJoystickIsPressed = Input.GetButton("Right Joystick Button");
        rightStick.color = rightJoystickIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        #endregion


        #region Triggers
        float lTriggerFloat = Input.GetAxis("Left Trigger");
        leftTrigger.color = new Color(lTriggerFloat, lTriggerFloat, lTriggerFloat);

        float rTriggerFloat = Input.GetAxis("Right Trigger");
        rightTrigger.color = new Color(rTriggerFloat, rTriggerFloat, rTriggerFloat);
        #endregion


        #region Bumpers
        bool leftBumperIsPressed = Input.GetButton("Left Bumper");
        leftBumper.color = leftBumperIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);

        bool rightBumperIsPressed = Input.GetButton("Right Bumper");
        rightBumper.color = rightBumperIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        #endregion


        #region D Pad
        float dpadHorizontal = Input.GetAxis("D Pad Horizontal");
        dPadLeft.color = dpadHorizontal < 0 ? new Color(1, 1, 1) : new Color(0, 0, 0);
        dPadRight.color = dpadHorizontal > 0 ? new Color(1, 1, 1) : new Color(0, 0, 0);

        float dpadVertical = Input.GetAxis("D Pad Vertical");
        dPadDown.color = dpadVertical < 0 ? new Color(1, 1, 1) : new Color(0, 0, 0);
        dPadUp.color = dpadVertical > 0 ? new Color(1, 1, 1) : new Color(0, 0, 0);
        #endregion


        #region A, B, X, Y Buttons
        bool aButtonIsPressed = Input.GetButton("A Button");
        bool bButtonIsPressed = Input.GetButton("B Button");
        bool xButtonIsPressed = Input.GetButton("X Button");
        bool yButtonIsPressed = Input.GetButton("Y Button");
        aButton.color = aButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        bButton.color = bButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        xButton.color = xButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        yButton.color = yButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        #endregion


        #region Back, Start Buttons
        bool backButtonIsPressed = Input.GetButton("Back Button");
        bool startButtonIsPressed = Input.GetButton("Start Button");
        backButton.color = backButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        startButton.color = startButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        #endregion
    }

}