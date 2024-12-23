using System;
using System.Collections;
using UnityEngine;
//using VoidProject;

namespace HJ
{
    public class LatentMonsterChest : MonoBehaviour
    {
        #region Variables
        //public SoundManager soundManager;

        private Animator chestAnimator;
        public Animator monsterAnimator;

        [SerializeField] private string animTrigger = "JumpScareTrigger";
        #endregion

        private void Start()
        {
            chestAnimator = GetComponent<Animator>();
        }

        public IEnumerator PlayAnim()
        {
            //체스트 오픈
            //soundManager.PlayClipByIndex(12, 1f);
            chestAnimator.SetTrigger(animTrigger);

            yield return new WaitForSeconds(0.1f);

            //점프스케어
            //soundManager.PlayClipByIndex(13, 1f);
            monsterAnimator.SetTrigger(animTrigger);
        }
    }
}