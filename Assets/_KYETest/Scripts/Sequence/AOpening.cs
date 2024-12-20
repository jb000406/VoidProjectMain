using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace VoidProject
{
    public class AOpening : WorldMenuUI
    {
        #region Variables
        //public GameObject thePlayer;
        public SceneFader fader;
        public GameObject Locomotion;

        //Effect
        public GameObject effect;
        
        //스타트 콜라이더
        public GameObject startCollider;  // StartCollider 참조

        #endregion

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            // 파티클 시스템 가져오기
            if (effect != null)
            {
                ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
                if (particleSystem == null)
                {
                    Debug.LogError("Effect GameObject에 ParticleSystem이 없습니다. 확인해주세요.");
                    return;
                }
            }
            else
            {
                Debug.LogError("Effect GameObject가 할당되지 않았습니다. 확인해주세요.");
                return;
            }

            if (fader == null)
            {
                Debug.LogError("SceneFader reference is missing. Please assign it in the inspector.");
                return;
            }
            if (!fader.gameObject.activeSelf)
            {
                fader.gameObject.SetActive(true);
            }
            // StartCollider 충돌 시작
            if (startCollider != null)
            {
                startCollider.SetActive(true);
            }
            else
            {
                Debug.LogError("StartCollider GameObject가 할당되지 않았습니다. 확인해주세요.");
            }
        }

        //오프닝 시퀀스
        IEnumerator PlaySequence()
        {
            // 1. 이펙트 시작
            fader.FromFade(0.5f);
            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            effect.SetActive(true);

            //플레이어 비 활성화
            Locomotion.SetActive(false);

/*            //2초후에 sequenceUI 닫기
            yield return new WaitForSeconds(2f);
            HideMenuUI();
*/
            //2초뒤에 sequenceImg
            yield return new WaitForSeconds(2f);
            ShowMenuImg(sequenceImg);

            //5. 2초후에 sequenceImg 닫기
            yield return new WaitForSeconds(1.5f);
            HideMenuImg();

            //이팩트 종로
           // fader.FromFade(3f); 
            yield return StartCoroutine(FadeOutParticles(particleSystem, 1.5f)); // 3초 동안 파티클 알파 감소
            effect.SetActive(false); // 파티클 비활성화

            // 콜라이더 비활성화 또는 제거
            if (startCollider != null)
            {
                startCollider.SetActive(false); // 콜라이더 비활성화
            }

            //플레이 캐릭터 활성화
            Locomotion.SetActive(true);
            Debug.Log("Sequence finished.");
        }

        private IEnumerator FadeOutParticles(ParticleSystem particleSystem, float duration)
        {
            float elapsedTime = 0f;

            // 파티클의 StartColor 가져오기
            var mainModule = particleSystem.main;
            Color startColor = mainModule.startColor.color; // 현재 StartColor
            float initialAlpha = startColor.a;

            // 페이드인 시작
            fader.FromFade(duration);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(initialAlpha, 0f, elapsedTime / duration); // 알파 값을 점진적으로 감소
                startColor.a = newAlpha;
                mainModule.startColor = startColor; // 변경된 알파 값을 적용

                yield return null; // 프레임마다 대기
            }

            // 최종적으로 알파 값을 0으로 설정
            startColor.a = 0f;
            mainModule.startColor = startColor;
        }
        // 플레이어가 StartCollider와 충돌하면 시퀀스를 시작하는 메서드
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // 플레이어가 StartCollider에 부딪히면 시퀀스 시작
                StartCoroutine(PlaySequence());
            }
        }
    }
}