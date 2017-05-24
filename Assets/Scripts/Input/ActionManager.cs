using UnityEngine;
using System.Collections.Generic;

/*
 * Input manager that listens for all input and delegates the inputs to ActionListeners. 
 * Anything that needs to be controlled by key presses should go through this class.
 * This class supports the following:
 * 1) Calling a MovementListener method every FixedUpdate tick with the Horizontal and Vertical axis movements.
 * 2) Calling a MouseMovementListener method every LateUpdate for mouse movement events.
 * 3) Calling a ButtonListener method when a key is pressed
 * 4) Calling a ButtonListener method when a key remains pressed
 * 5) Calling a ButtonListener method when a key is released.
 */
public class ActionManager : MyMonoBehaviour
{
    private static readonly int DEFAULT_SIZE = 5;
    private enum ButtonChangeType
    {
        ADD,
        REMOVE
    };
    private struct ButtonChange
    {
        private readonly ButtonChangeType _buttonChangeType;
        private readonly InputButton _inputButton;
        public ButtonChange(ButtonChangeType buttonChangeType, InputButton inputButton)
        {
            _buttonChangeType = buttonChangeType;
            _inputButton = inputButton;
        }

        public ButtonChangeType ButtonChangeType
        {
            get { return _buttonChangeType; }
        }
        public InputButton InputButton
        {
            get { return _inputButton; }
        }
    };

    private MovementListener movementListener;
    private MouseMovementListener mouseMovementListener;

    private IDictionary<int, List<ButtonListener>> startButtonListeners;
    private IDictionary<int, List<ButtonListener>> endButtonListeners;
    private IDictionary<int, List<ButtonListener>> continuousButtonListeners;
    private HashSet<InputButton> pendingButtons;
    private HashSet<InputButton> pressedButtons;
    private Queue<ButtonChange> pendingButtonChanges;

    protected override void MyAwake()
    {
        startButtonListeners = new Dictionary<int, List<ButtonListener>>();
        endButtonListeners = new Dictionary<int, List<ButtonListener>>();
        continuousButtonListeners = new Dictionary<int, List<ButtonListener>>();
        pendingButtons = new HashSet<InputButton>();
        pressedButtons = new HashSet<InputButton>();
        pendingButtonChanges = new Queue<ButtonChange>();

        movementListener = null;
        mouseMovementListener = null;
    }

    public void Update()
    {
        CheckForButtonPresses();
        CheckForButtonReleases();
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime)
    {
        ProcessPendingButtonChanges();
        ProcessPressedButtons();

        if (movementListener != null)
            movementListener(Input.GetAxis(InputButton.HORIZONTAL.Action), Input.GetAxis(InputButton.FORWARD.Action),
                Input.GetAxisRaw(InputButton.HORIZONTAL.Action), Input.GetAxisRaw(InputButton.FORWARD.Action), myDeltaTime);

        if (mouseMovementListener != null)
            mouseMovementListener(Input.GetAxis(InputButton.MOUSE_X.Action), Input.GetAxis(InputButton.MOUSE_Y.Action),
                Input.GetAxisRaw(InputButton.MOUSE_X.Action), Input.GetAxisRaw(InputButton.MOUSE_Y.Action), myDeltaTime);
    }

    public void RegisterMovementListener(MovementListener movementListener)
    {
        this.movementListener = movementListener;
    }

    public void RegisterMouseMovementListener(MouseMovementListener mouseMovementListener)
    {
        this.mouseMovementListener = mouseMovementListener;
    }

    public void RegisterStartButtonListener(InputButton inputButton, ButtonListener buttonListener)
    {
        pendingButtons.Add(inputButton);
        List<ButtonListener> buttonListeners = GetListFromMap(startButtonListeners, inputButton.Id);
        buttonListeners.Add(buttonListener);
    }

    public void RegisterEndButtonListener(InputButton inputButton, ButtonListener buttonListener)
    {
        pendingButtons.Add(inputButton);
        List<ButtonListener> buttonListeners = GetListFromMap(endButtonListeners, inputButton.Id);
        buttonListeners.Add(buttonListener);
    }

    public void RegisterContinuousButtonListener(InputButton inputButton, ButtonListener buttonListener)
    {
        RegisterStartButtonListener(inputButton, buttonListener);
        RegisterEndButtonListener(inputButton, buttonListener);
        List<ButtonListener> buttonListeners = GetListFromMap(continuousButtonListeners, inputButton.Id);
        buttonListeners.Add(buttonListener);
    }

    private List<ButtonListener> GetListFromMap(IDictionary<int, List<ButtonListener>> dictionary, int key)
    {
        List<ButtonListener> returnValue;
        if (!dictionary.TryGetValue(key, out returnValue))
        {
            returnValue = new List<ButtonListener>(DEFAULT_SIZE);
            dictionary.Add(key, returnValue);
        }

        return returnValue;
    }

    private void ProcessPendingButtonChanges()
    {
        while(pendingButtonChanges.Count > 0)
        {
            ButtonChange buttonChange = pendingButtonChanges.Dequeue();
            List<ButtonListener> buttonListeners;
            switch(buttonChange.ButtonChangeType)
            {
                case ButtonChangeType.ADD:
                    pendingButtons.Remove(buttonChange.InputButton);
                    pressedButtons.Add(buttonChange.InputButton);
                    if (startButtonListeners.TryGetValue(buttonChange.InputButton.Id, out buttonListeners))
                    {
                        foreach (ButtonListener buttonListener in buttonListeners)
                        {
                            buttonListener(buttonChange.InputButton);
                        }
                    }
                    break;
                case ButtonChangeType.REMOVE:
                    pressedButtons.Remove(buttonChange.InputButton);
                    pendingButtons.Add(buttonChange.InputButton);
                    if (endButtonListeners.TryGetValue(buttonChange.InputButton.Id, out buttonListeners))
                    {
                        foreach (ButtonListener buttonListener in buttonListeners)
                        {
                            buttonListener(buttonChange.InputButton);
                        }
                    }
                    break;
                default:
                    //Do nothing
                    break;
            }
        }
    }

    private void ProcessPressedButtons()
    {
        foreach (InputButton pressedButton in pressedButtons)
        {
            List<ButtonListener> buttonListeners;
            if (continuousButtonListeners.TryGetValue(pressedButton.Id, out buttonListeners))
            {
                foreach (ButtonListener buttonListener in buttonListeners)
                {
                    buttonListener(pressedButton);
                }
            }
        }
    }

    private void CheckForButtonReleases()
    {
        foreach(InputButton pressedButton in pressedButtons)
        {
            if (!Input.GetButton(pressedButton.Action))
                pendingButtonChanges.Enqueue(new ButtonChange(ButtonChangeType.REMOVE, pressedButton));
        }
    }

    private void CheckForButtonPresses()
    {
        foreach(InputButton pendingButton in pendingButtons)
        {
            if (Input.GetButton(pendingButton.Action))
                pendingButtonChanges.Enqueue(new ButtonChange(ButtonChangeType.ADD, pendingButton));
        }
    }
}