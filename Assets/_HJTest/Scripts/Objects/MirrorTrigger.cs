using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace HJ
{
    public class MirrorTrigger : MonoBehaviour
    {
        #region Variables
        public Transform playerCamera;  //�÷��̾� ī�޶�
        public Transform targetMirror;  //�ſ�
        public Transform targetObejct;  //��� ������Ʈ

        private MeshRenderer targetRenderer;
        private XRGrabInteractable targetInteractable;

        [SerializeField] public float activationAngle = 30f; //���� ����

        #endregion

        private void Start()
        {
            targetRenderer = targetObejct.GetComponent<MeshRenderer>();
            targetInteractable = targetObejct.GetComponent<XRGrabInteractable>();

            //�޽� ������ ��Ȱ��ȭ
            targetRenderer.enabled = false;

            //�̺�Ʈ �Լ� ���
            targetInteractable.selectEntered.AddListener(OnGrab);
        }

        private void OnTriggerStay(Collider other)
        {
            //�÷��̾� Ȯ��
            if (other.CompareTag("Player"))
            {
                //�÷��̾� ����
                Vector3 playerForward = playerCamera.forward;

                //Ÿ�ٰ��� ���� ���
                Vector3 directionToA = (targetMirror.position - playerCamera.position).normalized;

                //���� ��
                float angle = Vector3.Angle(playerForward, directionToA);

                if (angle <= activationAngle)
                {
                    //Ʈ���� �۵�
                    TriggerActivated();
                }
                else
                {
                    TriggerDeactivated();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                TriggerDeactivated();
            }
        }

        //Ȱ��ȭ
        private void TriggerActivated()
        {
            Debug.Log("�ٶ󺸴� ��");

            targetRenderer.enabled = true;
        }

        //��Ȱ��ȭ
        private void TriggerDeactivated()
        {
            Debug.Log("��Ȱ��ȭ");

            targetRenderer.enabled = false;
        }

        //From ī�޶� To Ÿ�� �����
        private void OnDrawGizmos()
        {
            if (playerCamera == null || targetMirror == null) return;

            // ����׿�: �÷��̾� ī�޶󿡼� A���� ���� ǥ��
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerCamera.position, targetMirror.position);
        }

        //������ Ÿ�� �޽������� Ȱ��ȭ
        private void OnGrab(SelectEnterEventArgs args)
        {
            targetRenderer.enabled = true;
            Destroy(this.gameObject);
        }
    }
}