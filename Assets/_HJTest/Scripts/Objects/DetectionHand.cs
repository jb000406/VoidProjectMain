using UnityEngine;


public class DetectionHand : MonoBehaviour
{
    #region Variables
    public Collider leftHand;
    public Collider rightHand;
    public Transform position;
    //[SerializeField] private bool isNearHand = false;

    [SerializeField] private float playerDistance = 0.2f;
    #endregion

    private void Start()
    {
        Collider collider = GetComponent<Collider>();
        
        leftHand.enabled = false;
        rightHand.enabled = false;
    }

    private void Update()
    {
        CollidersEnabled();
    }

    //���� ��ó�� �ִ��� �Ǻ�
    private bool IsNearPlayer()
    {
        float leftDistance = Vector3.Distance(leftHand.transform.position, position.position);
        float rightDistance = Vector3.Distance(rightHand.transform.position, position.position);
        return (leftDistance <= playerDistance) || (rightDistance <= playerDistance);
    }

    //�ݶ��̴� Ȱ��ȭ
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

    //������
    private void DisableCollidersDelayed()
    {
        leftHand.enabled = false;
        rightHand.enabled = false;

        //isNearHand = false;
    }
}
