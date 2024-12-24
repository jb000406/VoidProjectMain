using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VoidProject
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        #region Variables
        [Header("Player Controller")]
        public SceneFader fader;

        //체력
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 20;
        private float currentHealth;
        private bool isDeath = false;

        //데미지 효과
        [Header("Effects")]
        public GameObject damageFlash;      //데미지 플래쉬 효과

        //체력UI
        [Header("UI Settings")]
        public GameObject healthBarUI;
        [SerializeField] private Image healthBarImage;
        [SerializeField] private Transform playerCamera; // 플레이어의 카메라
        private Coroutine hideHealthBarCoroutine; // 체력바 숨기기 코루틴

        public GameUIManager gameUIManager;
        #endregion

        private void Start()
        {
            //초기화
            currentHealth = maxHealth;

            // 초기 체력 바 설정
            if (healthBarImage != null)
            {
                // fillAmount를 사용하여 이미지 채움 정도 설정
                healthBarImage.fillAmount = currentHealth / maxHealth;  // 체력 비율에 맞춰 설정
            }
            // 체력바 초기 숨김
            if (healthBarUI != null)
            {
                healthBarUI.SetActive(false);
            }
        }

        private void Update()
        { 
            if (playerCamera != null && healthBarUI != null)
            {
                // 체력바를 카메라 정면에 고정 (카메라 앞 1.5 단위 거리)
                healthBarUI.transform.position = playerCamera.position + playerCamera.forward * 0.5f;

                // 체력바가 항상 카메라를 향하도록 설정
                healthBarUI.transform.rotation = Quaternion.LookRotation(playerCamera.forward);
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"Player Health: {currentHealth}");

            // 체력 바 업데이트
            if (healthBarImage != null)
            {
                healthBarImage.fillAmount = currentHealth / maxHealth; 
            }

            // 체력바를 일시적으로 표시
            if (healthBarUI != null)
            {
                ShowHealthBar();
            }

            //데미지 효과
            StartCoroutine(DamageEffect());

            if (currentHealth < 0 && !isDeath)
            {
                isDeath = true; 
                Die();
                // 체력바와 대미지 플래시 비활성화
                if (healthBarUI != null)
                {
                    healthBarUI.SetActive(false);
                }

                if (damageFlash != null)
                {
                    damageFlash.SetActive(false);
                }
            }
        }

        void Die()
        {
            isDeath = true;
            gameUIManager.GameOver();
        }

        IEnumerator DamageEffect()
        {
            if (damageFlash != null)
            {
                damageFlash.SetActive(true);
                yield return new WaitForSeconds(1f);
                damageFlash.SetActive(false);
            }
        }
        void ShowHealthBar()
        {
            if (healthBarUI != null)
            {
                healthBarUI.SetActive(true);

                // 기존 숨기기 코루틴 중단
                if (hideHealthBarCoroutine != null)
                {
                    StopCoroutine(hideHealthBarCoroutine);
                }

                // 일정 시간 후 체력바 숨기기
                hideHealthBarCoroutine = StartCoroutine(HideHealthBarAfterDelay());
            }
        }

        IEnumerator HideHealthBarAfterDelay()
        {
            yield return new WaitForSeconds(3f); // 3초 후 숨김
            if (healthBarUI != null)
            {
                healthBarUI.SetActive(false);
            }
        }

    }
}