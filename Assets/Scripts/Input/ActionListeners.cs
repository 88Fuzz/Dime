/*
 * Method that listens to player movements. Method will be called on the FixedUpdate timestep.
 * detltaTime is the time since the last call to the Listener
 */
public delegate void MovementListener(float x, float z, float rawX, float rawZ, float deltaTime);

/*
 * Method that listens to button presses. Method can be called every FixedUpdate a button is pressed,
 * when a button is initially pressed, or when the button is released.
 */
public delegate void ButtonListener(InputButton button);

/*
 * Method that listens to mouse movements. Method will be called on the LateUpdate timestep.
 * detltaTime is the time since the last call to the Listener
 */
public delegate void MouseMovementListener(float x, float y, float rawX, float rawY, float deltaTime);