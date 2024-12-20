using UnityEngine;

namespace HJ
{
    public class DrawerOpen : MonoBehaviour
    {
        #region Variables

        #endregion

        #region Variabels
        [SerializeField] private Transform handle; // ������ Transform    [SerializeField]
        [SerializeField] private Transform lockHandlePosition; // ������ Transform

        [SerializeField]
        private float openRange = 2.0f; // �����̵� ������ ���� (���� ������ �Ÿ�)
        private Vector3 initialDrawerPosition; // ���� �ʱ� ��ġ
        private Vector3 initialHandlePosition; // �������� �ʱ� ��ġ
        private Vector3 lastHandleDirection; // �������� ������ ����
        private bool isGrabbing = false; // �׷� ���� Ȯ��

        //[SerializeField] private bool isLeft = true;
        #endregion

        private void Start()
        {
            // ���� �������� �ʱ� ��ġ ����
            initialDrawerPosition = transform.localPosition;
            initialHandlePosition = handle.localPosition;
        }

        void Update()
        {
            if (isGrabbing)
            {
                OpneDrawer();
                LockHandlePosition(); // �ڵ� ��ġ ����
            }
        }

        private void OpneDrawer()
        {
            // �ڵ��� z�� �̵� ���̸� ���
            float delta = handle.position.z - lastHandleDirection.z;

            // �� �����̵� ��ġ ������Ʈ
            Vector3 currentDoorPosition = transform.localPosition;
            currentDoorPosition.z -= delta;
            currentDoorPosition.z = Mathf.Clamp(currentDoorPosition.z, initialDrawerPosition.z, initialDrawerPosition.z + openRange);

            // �� �̵�
            transform.localPosition = currentDoorPosition;

            // ������ ���� ������Ʈ
            lastHandleDirection = handle.position;
        }


        /// <summary>
        /// ������ ��ġ�� ���� ����
        /// </summary>
        private void LockHandlePosition()
        {
            // ������ ��ġ�� ���� �Բ� ����
            handle.position = lockHandlePosition.position;
        }

        /// <summary>
        /// �����̸� ���� �� ȣ��
        /// </summary>
        public void StartGrabbing()
        {
            isGrabbing = true;

            // ������ ���� �ʱ�ȭ
            lastHandleDirection = handle.localPosition;
        }

        /// <summary>
        /// �����̸� ���� �� ȣ��
        /// </summary>
        public void StopGrabbing()
        {
            isGrabbing = false;

            // �����̸� �ʱ� ��ġ�� ����
            ResetHandlePosition();
        }

        /// <summary>
        /// �����̸� �ʱ� ��ġ�� ����
        /// </summary>
        private void ResetHandlePosition()
        {
            handle.localPosition = initialHandlePosition;
        }
    }
}