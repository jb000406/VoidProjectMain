using UnityEngine;
using VoidProject;

namespace VoidProject
{
    public class DoorOpenController : MonoBehaviour
    {
        #region Variables
        private Collider m_collider;
        private Animator animator;

        [SerializeField] private string animTriggerName = "OpenTrigger";
        [SerializeField] private float soundVolume = 1f;
        #endregion

        private void Start()
        {
            m_collider = GetComponent<Collider>();
            animator = GetComponent<Animator>();
        }

        public void DoorOpen()
        {
            //콜라이더 끄기
            m_collider.enabled = false;

            //애니메이션 재생
            
            animator.SetTrigger(animTriggerName);

            SoundManager.Instance.PlayClipAtPoint(14, transform.position, soundVolume);
        }
    }
}