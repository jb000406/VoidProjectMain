using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace VoidProject
{
    public class SceneFader : MonoBehaviour
    {
        #region Variables
        //Fader 이미지
        public Image image;
        public AnimationCurve curve;
        #endregion

        private void Awake()
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);  // SceneFader를 활성화
            }
        }
        private void Start()
        {
            //초기화: 시작시 화면을 검정색으로 시작
            image.color = new Color(0f, 0f, 0f, 1f);
        }

        public void FromFade(float delayTime = 0f)
        {
            //씬 시작시 페이드인 효과
            StartCoroutine(FadeIn(delayTime));
        }

        IEnumerator FadeIn(float delayTime)
        {
            if(delayTime > 0f)
            {
                yield return new WaitForSeconds(delayTime);
            }

            //1초동안 image a 1-> 0
            float t = 1f;

            while (t > 0)
            {
                t -= Time.deltaTime;
                float a = curve.Evaluate(t);
                image.color = new Color(0f, 0f, 0f, a);
                yield return null;    
            }
        }

        public void FadeTo(string sceneName)
        {
            StartCoroutine(FadeOut(sceneName));
        }

        public void FadeTo(int sceneNumber)
        {
            StartCoroutine(FadeOut(sceneNumber));
        }
        public void FadeTo(float delayTime = 0f)
        {
            //씬 시작시 페이드인 효과
            StartCoroutine(FadeOut(delayTime));
        }

        IEnumerator FadeOut(string sceneName)
        {
            //1초동안 image a 0-> 1
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime;
                float a = curve.Evaluate(t);
                image.color = new Color(0f, 0f, 0f, a);
                yield return null;
            }
        }
        IEnumerator FadeOut(float delayTime)
        {
            if (delayTime > 0f)
            {
                yield return new WaitForSeconds(delayTime);
            }

            //1초동안 image a 1-> 0
            float t = 1f;

            while (t > 0)
            {
                t -= Time.deltaTime;
                float a = curve.Evaluate(t);
                image.color = new Color(0f, 0f, 0f, a);
                yield return null;
            }
        }

        IEnumerator FadeOut(int sceneNumber)
        {
            //1초동안 image a 0-> 1
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime;
                float a = curve.Evaluate(t);
                image.color = new Color(0f, 0f, 0f, a);
                yield return 0f;
            }

            //다음씬 로드

            if(sceneNumber >= 0)
            {
                SceneManager.LoadScene(sceneNumber);
            }
            //SceneManager.LoadScene(sceneNumber);
        }

    }
}