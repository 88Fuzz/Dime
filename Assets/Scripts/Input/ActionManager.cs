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
    private MovementListener movementListener;
    private MouseMovementListener mouseMovementListener;

    private IDictionary<int, ButtonListener> startButtonListeners;
    private IDictionary<int, ButtonListener> endButtonListeners;
    private IDictionary<int, ButtonListener> continuousButtonListeners;
    private HashSet<InputButton> pendingButtons;
    private HashSet<InputButton> pressedButtons;

    public void Awake()
    {
        startButtonListeners = new Dictionary<int, ButtonListener>();
        endButtonListeners = new Dictionary<int, ButtonListener>();
        continuousButtonListeners = new Dictionary<int, ButtonListener>();
        pendingButtons = new HashSet<InputButton>();
        pressedButtons = new HashSet<InputButton>();

        movementListener = null;
        mouseMovementListener = null;
    }

    //TODO figure out if this should be FixedUpdate or simply Update
    public void Update()
    {
        if (movementListener != null)
            movementListener(Input.GetAxis(InputButton.HORIZONTAL.Action), Input.GetAxis(InputButton.FORWARD.Action),
                Input.GetAxisRaw(InputButton.HORIZONTAL.Action), Input.GetAxisRaw(InputButton.FORWARD.Action));

        checkPressedButtons();
        checkPendingButtons();
    }

    public void LateUpdate()
    {
        if (mouseMovementListener != null)
            mouseMovementListener(Input.GetAxis(InputButton.MOUSE_X.Action), Input.GetAxis(InputButton.MOUSE_Y.Action),
                Input.GetAxisRaw(InputButton.MOUSE_X.Action), Input.GetAxisRaw(InputButton.MOUSE_Y.Action));
    }

    public void registerMovementListener(MovementListener movementListener)
    {
        this.movementListener = movementListener;
    }

    public void registerMouseMovementListener(MouseMovementListener mouseMovementListener)
    {
        this.mouseMovementListener = mouseMovementListener;
    }

    public void registerStartButtonListener(InputButton inputButton, ButtonListener buttonListener)
    {
        pendingButtons.Add(inputButton);
        startButtonListeners[inputButton.Id] = buttonListener;
    }

    public void registerEndButtonListener(InputButton inputButton, ButtonListener buttonListener)
    {
        pendingButtons.Add(inputButton);
        endButtonListeners[inputButton.Id] = buttonListener;
    }

    public void registerContinuousButtonListener(InputButton inputButton, ButtonListener buttonListener)
    {
        pendingButtons.Add(inputButton);
        startButtonListeners[inputButton.Id] = buttonListener;
        continuousButtonListeners[inputButton.Id] = buttonListener;
        endButtonListeners[inputButton.Id] = buttonListener;
    }

    private void checkPressedButtons()
    {
        LinkedList<InputButton> removeButtons = new LinkedList<InputButton>();
        foreach (InputButton pressedButton in pressedButtons)
        {
            if (!Input.GetButton(pressedButton.Action))
            {
                ButtonListener buttonListener;
                if (endButtonListeners.TryGetValue(pressedButton.Id, out buttonListener))
                {
                    buttonListener(pressedButton);
                }
                pendingButtons.Add(pressedButton);
                removeButtons.AddFirst(pressedButton);
            }
            else
            {
                ButtonListener buttonListener;
                if (continuousButtonListeners.TryGetValue(pressedButton.Id, out buttonListener))
                {
                    buttonListener(pressedButton);
                }
            }
        }
        removeFromSet(pressedButtons, removeButtons);
    }

    private void checkPendingButtons()
    {
        LinkedList<InputButton> removeButtons = new LinkedList<InputButton>();
        foreach (InputButton pendingButton in pendingButtons)
        {
            if (Input.GetButton(pendingButton.Action))
            {
                ButtonListener buttonListener;
                if (startButtonListeners.TryGetValue(pendingButton.Id, out buttonListener))
                {
                    buttonListener(pendingButton);
                    pressedButtons.Add(pendingButton);
                    removeButtons.AddFirst(pendingButton);
                }
            }
        }
        removeFromSet(pendingButtons, removeButtons);
    }

    private void removeFromSet(HashSet<InputButton> set, LinkedList<InputButton> removeList)
    {
        foreach (InputButton removeButton in removeList)
        {
            set.Remove(removeButton);
        }
    }
}