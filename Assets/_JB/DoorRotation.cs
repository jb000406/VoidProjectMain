using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleDoorInteraction : MonoBehaviour
{
    public Transform doorObject;
    private Transform leftController;
    private Transform rightController;
    private InputActionProperty leftGrabAction;
    private InputActionProperty rightGrabAction;
    public float maxAngle = 90f; // 90도까지 회전 가능
    public float minAngle = -90f;
    public float rotationSpeed = 3f;
    public float doorCloseDistance = 0.6f;
    public AudioSource doorAudio;

    private bool isGrabbing = false;
    private float initialDistance;
    private float grabReleaseTime = 0f;
    private bool isClosing = false;
    public float autoCloseDelay = 2f;
    public float closeSpeed = 5f;

    private void Start()
    {
        leftController = GameManager.LeftController;
        rightController = GameManager.RightController;
        leftGrabAction = GameManager.LeftGrabAction;
        rightGrabAction = GameManager.RightGrabAction;
    }

    private void Update()
    {
        HandleGrabInput();
        if (!isGrabbing && !isClosing && Time.time - grabReleaseTime >= autoCloseDelay)
        {
            isClosing = true;
        }
        if (isClosing)
        {
            AutoCloseDoor();
        }
    }

    private void HandleGrabInput()
    {
        bool leftgrabPressed = leftGrabAction.action.ReadValue<float>() > 0.5f;
        bool rightgrabPressed = rightGrabAction.action.ReadValue<float>() > 0.5f;
        Transform controller = leftgrabPressed ? leftController : rightController;
        Vector3 closestPoint = doorObject.GetComponent<Collider>().ClosestPoint(controller.position);
        float distanceXZ = Vector3.Distance(new Vector3(closestPoint.x, 0, closestPoint.z),
                                            new Vector3(controller.position.x, 0, controller.position.z));

        if (leftgrabPressed || rightgrabPressed)
        {
            if (!isGrabbing && distanceXZ < doorCloseDistance)
            {
                isGrabbing = true;
                initialDistance = distanceXZ;
                isClosing = false;
            }
            if (isGrabbing)
            {
                PlayDoorSound();
                RotateDoor(distanceXZ, controller);
            }
        }

        if ((!leftgrabPressed && !rightgrabPressed) || distanceXZ >= doorCloseDistance)
        {
            if (isGrabbing)
            {
                isGrabbing = false;
                grabReleaseTime = Time.time;
                StopDoorSound();
            }
        }
    }

    private void RotateDoor(float distance, Transform controller)
    {
        float distanceDelta = initialDistance - distance;
        Vector3 controllerDirection = doorObject.InverseTransformDirection(controller.forward);
        float direction = Vector3.Dot(controllerDirection, Vector3.right);
        float rotationDelta = distanceDelta * rotationSpeed * Mathf.Sign(-direction);
        float currentAngle = doorObject.localEulerAngles.y;
        currentAngle = (currentAngle > 180) ? currentAngle - 360 : currentAngle;
        float targetAngle = Mathf.Clamp(currentAngle + rotationDelta, minAngle, maxAngle);
        doorObject.localRotation = Quaternion.Euler(0, targetAngle, 0);
    }

    private void AutoCloseDoor()
    {
        float currentAngle = doorObject.localEulerAngles.y;
        currentAngle = (currentAngle > 180) ? currentAngle - 360 : currentAngle;
        float targetAngle = Mathf.Lerp(currentAngle, 0f, Time.deltaTime * closeSpeed);
        doorObject.localRotation = Quaternion.Euler(0, targetAngle, 0);

        if (Mathf.Abs(currentAngle) < 0.1f)
        {
            doorObject.localRotation = Quaternion.identity;
            isClosing = false;
        }
    }

    public void PlayDoorSound()
    {
        if (doorAudio != null && !doorAudio.isPlaying)
        {
            doorAudio.loop = true;
            doorAudio.Play();
        }
    }

    public void StopDoorSound()
    {
        if (doorAudio != null && doorAudio.isPlaying)
        {
            doorAudio.loop = false;
            doorAudio.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌");
        isClosing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("접촉" + other.name);
        isClosing = false;
    }
}
