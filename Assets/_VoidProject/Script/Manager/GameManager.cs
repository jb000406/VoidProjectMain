using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public static Transform LeftController { get; private set; }
    public static Transform RightController { get; private set; }
    public static InputActionProperty LeftGrabAction {  get; private set; }
    public static InputActionProperty RightGrabAction {  get; private set; }

    [SerializeField] private Transform leftController;
    [SerializeField] private Transform rightController;
    [SerializeField] private InputActionProperty leftGrabAction;
    [SerializeField] private InputActionProperty rightGrabAction;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        InitializeProperty();
    }

    private void InitializeProperty()
    {
        LeftController = leftController;
        RightController = rightController;
        LeftGrabAction = leftGrabAction;
        RightGrabAction = rightGrabAction;
    }
}
