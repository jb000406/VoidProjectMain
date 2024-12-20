using System.Collections;
using UnityEngine;


namespace VoidProject
{
    public class IndicatorUI : MonoBehaviour
    {
        #region Variables
        public GameObject indicatorUI;
        private CanvasGroup indicatorCanvasGroup;
        #endregion

        private void Start()
        {
            // CanvasGroup 설정
            InitializeIndicatorUI();
        }

        // 인디케이터 UI 초기화
        private void InitializeIndicatorUI()
        {
            if (indicatorUI != null)
            {
                indicatorCanvasGroup = indicatorUI.GetComponent<CanvasGroup>();
                if (indicatorCanvasGroup == null)
                {
                    indicatorCanvasGroup = indicatorUI.AddComponent<CanvasGroup>();
                }
                indicatorCanvasGroup.alpha = 0; // 처음엔 투명
                indicatorUI.SetActive(false);   // 비활성화 상태로 시작
            }
        }

        // 인디케이터 UI 활성화/비활성화
        public void SetIndicatorVisibility(bool isVisible)
        {
            if (indicatorUI != null)
            {
                indicatorUI.SetActive(true); // 활성화
                StopAllCoroutines();
                StartCoroutine(FadeIndicator(isVisible));
            }
            else
            {
                Debug.LogWarning("Indicator UI가 설정되지 않았습니다.");
            }
        }

        // CanvasGroup으로 Fade 효과 구현
        private IEnumerator FadeIndicator(bool fadeIn)
        {
            if (indicatorCanvasGroup == null) yield break;

            float duration = 0.5f; // 페이드 지속 시간
            float targetAlpha = fadeIn ? 1f : 0f;
            float startAlpha = indicatorCanvasGroup.alpha;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                indicatorCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
                yield return null;
            }

            indicatorCanvasGroup.alpha = targetAlpha;

            if (!fadeIn) indicatorUI.SetActive(false); // 비활성화
        }
    }
}