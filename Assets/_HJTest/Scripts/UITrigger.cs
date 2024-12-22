using System.Collections;
using TMPro;
using UnityEngine;

namespace VoidProject
{
    public class UITrigger : MonoBehaviour
    {
        #region Variables
        public GameObject sequenceCanvas;
        public TextMeshProUGUI text;
        [SerializeField] private string sequenceText = "text";
        [SerializeField] private float offDelay = 10f;
        #endregion

        private void Start()
        {
            //트리거 설정
            Collider c = GetComponent<Collider>();
            c.isTrigger = true;

            //시퀀스
            sequenceCanvas.SetActive(false);
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
            sequenceCanvas.SetActive(true);
            text.text = sequenceText;

            yield return new WaitForSeconds(offDelay);

            sequenceCanvas.SetActive(false);
            text.text = "";

            Destroy(this.gameObject);
        }
    }
}