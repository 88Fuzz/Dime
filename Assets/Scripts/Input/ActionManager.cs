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
public class ActionManager : MonoBehaviour
{
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

    private IDictionary<int, ButtonListener> startButtonListeners;
    private IDictionary<int, ButtonListener> endButtonListeners;
    private IDictionary<int, ButtonListener> continuousButtonListeners;
    private HashSet<InputButton> pendingButtons;
    private HashSet<InputButton> pressedButtons;
    private Queue<ButtonChange> pendingButtonChanges;

    public void Awake()
    {
        startButtonListeners = new Dictionary<int, ButtonListener>();
        endButtonListeners = new Dictionary<int, ButtonListener>();
        continuousButtonListeners = new Dictionary<int, ButtonListener>();
        pendingButtons = new HashSet<InputButton>();
        pressedButtons = new HashSet<InputButton>();
        pendingButtonChanges = new Queue<ButtonChange>();

        movementListener = null;
        mouseMovementListener = null;
    }

    //TODO, start here and test the shit out this code with unit tests. After you are done with that shit,
        //Continue moving things out of the Update loop and into the FixedUpdate loop
    public void Update()
    {
        CheckForButtonPresses();
        CheckForButtonReleases();
    }

    public void FixedUpdate()
    {
        ProcessPendingButtonChanges();
        ProcessPressedButtons();

        if (movementListener != null)
            movementListener(Input.GetAxis(InputButton.HORIZONTAL.Action), Input.GetAxis(InputButton.FORWARD.Action),
                Input.GetAxisRaw(InputButton.HORIZONTAL.Action), Input.GetAxisRaw(InputButton.FORWARD.Action));

        if (mouseMovementListener != null)
            mouseMovementListener(Input.GetAxis(InputButton.MOUSE_X.Action), Input.GetAxis(InputButton.MOUSE_Y.Action),
                Input.GetAxisRaw(InputButton.MOUSE_X.Action), Input.GetAxisRaw(InputButton.MOUSE_Y.Action));
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
        startButtonListeners[inputButton.Id] = buttonListener;
    }

    public void RegisterEndButtonListener(InputButton inputButton, ButtonListener buttonListener)
    {
        pendingButtons.Add(inputButton);
        endButtonListeners[inputButton.Id] = buttonListener;
    }

    public void RegisterContinuousButtonListener(InputButton inputButton, ButtonListener buttonListener)
    {
        pendingButtons.Add(inputButton);
        startButtonListeners[inputButton.Id] = buttonListener;
        continuousButtonListeners[inputButton.Id] = buttonListener;
        endButtonListeners[inputButton.Id] = buttonListener;
    }

    private void ProcessPendingButtonChanges()
    {
        while(pendingButtonChanges.Count > 0)
        {
            ButtonChange buttonChange = pendingButtonChanges.Dequeue();
            ButtonListener buttonListener;
            switch(buttonChange.ButtonChangeType)
            {
                case ButtonChangeType.ADD:
                    pendingButtons.Remove(buttonChange.InputButton);
                    if (startButtonListeners.TryGetValue(buttonChange.InputButton.Id, out buttonListener))
                    {
                        buttonListener(buttonChange.InputButton);
                        pressedButtons.Add(buttonChange.InputButton);
                    }
                    break;
                case ButtonChangeType.REMOVE:
                    pressedButtons.Remove(buttonChange.InputButton);
                    if (endButtonListeners.TryGetValue(buttonChange.InputButton.Id, out buttonListener))
                    {
                        buttonListener(buttonChange.InputButton);
                        pendingButtons.Add(buttonChange.InputButton);
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
            ButtonListener buttonListener;
            if (continuousButtonListeners.TryGetValue(pressedButton.Id, out buttonListener))
                buttonListener(pressedButton);
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