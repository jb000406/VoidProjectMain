using System;
using System.Collections;
using UnityEngine;

namespace VoidProject
{
    public class LatentMonsterChest : MonoBehaviour
    {
        #region Variables
        private Animator chestAnimator;
        public Animator monsterAnimator;

        [SerializeField] private string animTrigger = "JumpScareTrigger";
        [SerializeField] private float soundVolume = 1f;

        public GameObject triggerObj;
        #endregion

        private void Start()
        {
            chestAnimator = GetComponent<Animator>();
        }

        public IEnumerator PlayAnim()
        {
            //체스트 오픈
            chestAnimator.SetTrigger(animTrigger);

            yield return new WaitForSeconds(0.1f);

            //점프스케어
            SoundManager.Instance.PlayClipAtPoint(20, transform.position, soundVolume);
            monsterAnimator.SetTrigger(animTrigger);

            Destroy(triggerObj);
        }
    }
}