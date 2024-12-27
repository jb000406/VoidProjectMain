using UnityEngine;
using VoidProject;


public class DetectionHand : MonoBehaviour
{
    #region Variables
    private Collider leftHand;
    private Collider rightHand;
    public Transform position;
    //[SerializeField] private bool isNearHand = false;

    [SerializeField] private float playerDistance = 0.2f;
    #endregion

    private void Start()
    {
        Collider collider = GetComponent<Collider>();

        leftHand = GameManager.LeftController.GetComponent<Collider>();
        rightHand = GameManager.RightController.GetComponent<Collider>();

        leftHand.enabled = false;
        rightHand.enabled = false;
    }

    private void Update()
    {
        CollidersEnabled();
    }

    //손이 근처에 있는지 판별
    private bool IsNearPlayer()
    {
        float leftDistance = Vector3.Distance(leftHand.transform.position, position.position);
        float rightDistance = Vector3.Distance(rightHand.transform.position, position.position);
        return (leftDistance <= playerDistance) || (rightDistance <= playerDistance);
    }

    //콜라이더 활성화
    private void CollidersEnabled()
    {
        if (IsNearPlayer())
        {
            leftHand.enabled = true;
            rightHand.enabled = true;

            //isNearHand = true;
        }
        else
        {
            DisableCollidersDelayed();
        }
    }

    //딜레이
    private void DisableCollidersDelayed()
    {
        leftHand.enabled = false;
        rightHand.enabled = false;
    }
}
