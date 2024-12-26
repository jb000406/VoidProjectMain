using UnityEngine;
using UnityEngine.UI;

namespace HJ
{
    public class PolishObject : MonoBehaviour
    {
        #region Variables
        private int polishCount;                                //닦는 횟수
        [SerializeField] private int maxPolishCount = 5;        //글자가 완전히 나타나는 값
        [SerializeField] private float conditioningValue = 2f;  //조정값

        public CanvasGroup canvasGroup;
        private AudioSource audioSource;

        public GameObject triggerObj;
        #endregion


        private void Start()
        {
            polishCount = 0;
            canvasGroup.alpha = 0;

            audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Polishing();
                triggerObj.SetActive(true);
            }
        }

        //닦을때 마다 점점 진해지기
        private void Polishing()
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if (polishCount == maxPolishCount)
                return;

            polishCount++;

            //진행도
            float progress = Mathf.Pow((float)polishCount / maxPolishCount, conditioningValue); //변화 곡선

            //캔버스그룹 알파값 조정
            canvasGroup.alpha = progress;
        }
    }
}