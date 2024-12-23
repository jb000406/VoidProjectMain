using System.Collections;
using TMPro;
using UnityEngine;

namespace VoidProject
{
    public class UITrigger : WorldMenuUI
    {
        #region Variables
        //public GameObject sequenceCanvas;
       //public TextMeshProUGUI text;
        [SerializeField] private string sequenceText = "text";
        [SerializeField] private float offDelay = 10f;
        //플레이어
        public GameObject Locomotion;
        //private Transform playerCamera;

        //콜라이더
        private Collider UIcollider;
        #endregion

        protected override void Start()
        {
            base.Start();
            //트리거 설정
            UIcollider = GetComponent<Collider>();
            UIcollider.gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                StartCoroutine(ActiveSequence());
            }
        }

        private IEnumerator ActiveSequence()
        {
            Locomotion.SetActive(false);
            ShowMenuUI(sequenceText);
            yield return new WaitForSeconds(offDelay);
            HideMenuUI();
            UIcollider.gameObject.SetActive(false);
            Locomotion.SetActive(true);
            Debug.Log("Sequence finished.");
        }
    }
}