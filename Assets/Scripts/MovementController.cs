using UnityEngine;

public class MovementController : MonoBehaviour
{
    //References
    [Header("References")]
    public Transform trans;
    public Transform modelTrans;
    public Transform weaponHolderTrans;
    public CharacterController characterController;
    //Movement
    [Header("Movement")]
    [Tooltip("Units moved per second at maximum speed.")]
    public float movespeed = 24;
    [Tooltip("Time, in seconds, to reach maximum speed.")]
    public float timeToMaxSpeed = 0.26f;

    private float VelocityGainPerSecond
    { 
        get
        { 
            return movespeed / timeToMaxSpeed;
        }
    }

    [Tooltip("Time, in seconds, to go from maximum speed to stationary.")]
    public float timeToLoseMaxSpeed = 0.2f;

    private float VelocityLossPerSecond
    {
        get
        {
            return movespeed / timeToLoseMaxSpeed;
        }
    }
    [Tooltip("Multiplier for momentum when attempting to move in a direction opposite the current traveling direction (e.g. trying to move right when already moving left).")]
    public float reverseMomentumMultiplier = 2.2f;
    private Vector3 movementVelocity = Vector3.zero;

    [Header("Mouse look")]
    public float mouseSensitivity = 10.0f;
    public Transform cameraHolder;
    private float verticalLookRotation = 0f;

    private void Movement()
    {

        // Vertical movement (Y axis)
        if (Input.GetKey(KeyCode.V))
        {
            // If we are already moving up
            if (movementVelocity.y >= 0)
                movementVelocity.y = Mathf.Min(movespeed, movementVelocity.y + VelocityGainPerSecond * Time.deltaTime);
            else
                movementVelocity.y = Mathf.Min(0, movementVelocity.y + VelocityGainPerSecond * Time.deltaTime * reverseMomentumMultiplier);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            // If we are already moving up
            if (movementVelocity.y > 0)
                movementVelocity.y = Mathf.Max(0, movementVelocity.y - VelocityGainPerSecond * Time.deltaTime * reverseMomentumMultiplier);
            else
                movementVelocity.y = Mathf.Max(-movespeed, movementVelocity.y - VelocityGainPerSecond * Time.deltaTime);
        }
        else
        {
            if (movementVelocity.y > 0)
                movementVelocity.y = Mathf.Max(0, movementVelocity.y - VelocityGainPerSecond * Time.deltaTime);
            else
                movementVelocity.y = Mathf.Min(0, movementVelocity.y + VelocityGainPerSecond * Time.deltaTime);

        }
        // Forward and backward movement (Z axis)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (movementVelocity.z >= 0) // If we are already moving forward
                movementVelocity.z = Mathf.Min(movespeed, movementVelocity.z + VelocityGainPerSecond*Time.deltaTime);
            
            else // If we are moving back
                //Increase Z velocity by VelocityGainPerSecond, using the reverseMomentumMultiplier, but don't raise higher than 0:
                movementVelocity.z = Mathf.Min(0, movementVelocity.z + VelocityGainPerSecond * Time.deltaTime * reverseMomentumMultiplier);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // If we are already moving forward
            if (movementVelocity.z > 0)
                movementVelocity.z = Mathf.Max(0, movementVelocity.z - VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
            //If we're moving back or not moving at all
            else
                movementVelocity.z = Mathf.Max(-movespeed, movementVelocity.z - VelocityGainPerSecond * Time.deltaTime);
        }
        else //If we're moving back or not moving at all
        {
            //We must bring the Z velocity back to 0 over time.
            if (movementVelocity.z > 0)
                //Decrease Z velocity by VelocityLossPerSecond, but don't go any lower than 0:
                movementVelocity.z = Mathf.Max(0, movementVelocity.z - VelocityLossPerSecond * Time.deltaTime);
            else //If we're moving backward,
                //Increase Z velocity (back towards 0) by VelocityLossPerSecond, but don't go any higher than 0:
                movementVelocity.z = Mathf.Min(0, movementVelocity.z + VelocityGainPerSecond * Time.deltaTime);
        }

        // Lateral movement (X axis)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // If we are already moving right
            if (movementVelocity.x >= 0)
                //Increase X velocity by VelocityGainPerSecond, but don't go higher than 'movespeed':
                movementVelocity.x = Mathf.Min(movespeed, movementVelocity.x + VelocityGainPerSecond * Time.deltaTime);
            else // If we are already moving left
                //Increase x velocity by VelocityGainPerSecond, using the reverseMomentumMultiplier, but don't raise higher than 0:
                movementVelocity.x = Mathf.Min(0, movementVelocity.x + VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
        }
        // If A or left arrow keys are held:
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            // If we are already moving right
            if (movementVelocity.x > 0)
                movementVelocity.x = Mathf.Max(0, movementVelocity.x - VelocityGainPerSecond * Time.deltaTime * reverseMomentumMultiplier);
            // If we are moving left or not moving at all
            else
                movementVelocity.x = Mathf.Max(-movespeed, movementVelocity.x - VelocityGainPerSecond * Time.deltaTime);
        }
        else // If neither right nor left are being held
        {
            // We must bring the X velocity back to 0 over time.
            // If we're moving right.
            if (movementVelocity.x > 0)
                //Decrease X velocity by VelocityLossPerSecond, but don't go any lower than 0:
                movementVelocity.x = Mathf.Max(0, movementVelocity.x - VelocityLossPerSecond * Time.deltaTime);
            else
                //Increase X velocity (back towards 0) by VelocityLossPerSecond, but don't go any higher than 0:
                movementVelocity.x = Mathf.Min(0, movementVelocity.x + VelocityLossPerSecond * Time.deltaTime);
        }
        
        // Applying movement and rotation to game object.
        if (movementVelocity.x != 0 || movementVelocity.z != 0 || movementVelocity.y != 0)
        {
            // applying the movement velocity
            Vector3 forwardProj = Vector3.ProjectOnPlane(trans.forward, Vector3.down).normalized;
            Vector3 lateralProj = Vector3.ProjectOnPlane(trans.right, Vector3.down).normalized;
            Vector3 relativeMovement = lateralProj * movementVelocity.x + forwardProj * movementVelocity.z;
            Vector3 fullVelocity = relativeMovement + Vector3.up * movementVelocity.y;
            characterController.Move(fullVelocity * Time.deltaTime);
            // Keeping the model holder rotated towards the last movement direction
            // Now we calculate the rotation according to the pressed keys
            float tiltX = 0f;
            float tiltZ = 0f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                tiltX = 10f;
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                tiltX = -10;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                tiltZ = 10f;
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                tiltZ = -10f;
            //Quaternion targetRotation = Quaternion.Euler(tiltX, modelTrans.rotation.eulerAngles.y, tiltZ);
            Quaternion targetRotation = Quaternion.Euler(tiltX, trans.rotation.eulerAngles.y, tiltZ);
            modelTrans.rotation = Quaternion.Slerp(modelTrans.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        trans.Rotate(Vector3.up * mouseX);
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);
        cameraHolder.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
        weaponHolderTrans.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
        //trans.Rotate(Vector3.right * -mouseY);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;   
    }

    private void Update()
    {
        Movement();
        MouseMovement();
    }
}