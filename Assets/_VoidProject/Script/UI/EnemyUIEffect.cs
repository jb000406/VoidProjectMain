using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VoidProject
{
    public class EnemyUIEffect : MonoBehaviour
    {
        #region Variables
        [Header("Enemy Position UI")]
        //public Canvas enemyPositionCanvas;
        //public Transform enemyTransform;                
        public Image warningImage;               // 경고 UI 이미지
        [SerializeField] private float detectDistance = 10f; // 적이 감지되는 최대 거리
        [SerializeField] private float warningDuration = 4f; // 경고 UI 표시 시간
        private Transform playerTransform;              // 플레이어의 Transform
        private bool isWarningActive = false;           // 경고 UI가 활성화된 상태인지 체크
        private string monsterTag = "Monster";      //몬스터 태그 
        #endregion

        void Start()
        {
            playerTransform = Camera.main.transform; // 또는 플레이어의 위치가 필요하다면 그에 맞게 설정
        }

        void Update()
        {
            // 레이캐스트를 통해 플레이어의 시선 방향에 몬스터가 있는지 확인
            RaycastHit hit;
            Vector3 directionToEnemy = (playerTransform.position - transform.position).normalized;

            if (Physics.Raycast(playerTransform.position, directionToEnemy, out hit, detectDistance))
            {
                // 레이가 몬스터에 맞았을 때
                if (hit.transform.CompareTag(monsterTag))
                {
                    // 몬스터가 시야 내에 있을 경우 경고 UI 표시
                    if (!isWarningActive)
                    {
                        ShowWarning(true);
                        isWarningActive = true;
                        StartCoroutine(HideWarningAfterDelay());
                    }
                }
            }
            else
            {
                // 시야에 몬스터가 없으면 경고 UI 숨기기
                if (isWarningActive)
                {
                    ShowWarning(false);
                    isWarningActive = false;
                }
            }
        }

        // 경고 UI를 활성화하거나 비활성화하는 함수
        void ShowWarning(bool show)
        {
            warningImage.gameObject.SetActive(show);
        }

        // 일정 시간 후 경고 UI 숨기기
        IEnumerator HideWarningAfterDelay()
        {
            yield return new WaitForSeconds(warningDuration);
            ShowWarning(false);
        }
    }
}