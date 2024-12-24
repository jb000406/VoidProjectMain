using System.Collections.Generic;
using UnityEngine;

namespace VoidProject
{
    public class CandlePuzzleManager : MonoBehaviour
    {
        #region Variables
        public GameObject CandlesGo;
        private Candle[] Candles;

        private int currentOrder = 0;
        private int candleOrder = 0;

        [SerializeField] private GameObject doorParent;
        [SerializeField] private GameObject[] doorChild;

        [SerializeField] private GameObject fakeDoorParent;
        [SerializeField] private GameObject[] fakeDoorChild;

        public GameObject grave;
        #endregion

        private void Start()
        {
            Candles = CandlesGo.GetComponentsInChildren<Candle>();

            //초기화: 모든 촛불 끄기 및 상태 초기화
            foreach (var candle in Candles)
            {
                candle.Extinguish();
                candle.isLit = false;
                candle.igniteOrder = candleOrder;
                candleOrder++;
            }

            currentOrder = 0; //초기 순서 설정

            if (doorParent != null && fakeDoorParent != null)
            {
                int doorCount = doorParent.transform.childCount;
                int fakeDoorCount = fakeDoorParent.transform.childCount;

                doorChild = new GameObject[doorCount];
                fakeDoorChild = new GameObject[fakeDoorCount];

                for (int i = 0; i < doorCount; i++)
                {
                    doorChild[i] = doorParent.transform.GetChild(i).gameObject;
                    fakeDoorChild[i] = fakeDoorParent.transform.GetChild(i).gameObject;
                }
            }
        }

        // 촛불 점화 시도
        public void AttemptIgnite(Candle candle)
        {
            if (candle.igniteOrder == currentOrder) //올바른 순서로 켰다면
            {
                Debug.Log($"점화 순서 맞음: {candle.name}");
                candle.Ignite(); //촛불 점화
                currentOrder++; //다음 순서로 진행
                Debug.Log(currentOrder);

                //모든 순서 맞으면 퍼즐 완료
                if (currentOrder >= Candles.Length)
                {
                    OnPuzzleComplete();
                }
            }
            else //순서 틀림
            {
                Debug.Log($"틀림 리셋");
                ResetPuzzle();
            }
        }

        private void ResetPuzzle()
        {
            foreach (var candle in Candles)
            {
                candle.Extinguish(); //모든 촛불 끄기
                candle.isLit = false; //상태 초기화
            }
            currentOrder = 0; //순서 초기화
        }

        private void OnPuzzleComplete()
        {
            Debug.Log("=========퍼즐 성공=========");

            //퍼즐 완료 동작 추가
            for (int i = 0; i < doorChild.Length; i++)
            {
                doorChild[i].gameObject.SetActive(true);
                fakeDoorChild[i].gameObject.SetActive(false);
            }

            grave.SetActive(true);

            SoundManager.Instance.PlayClipAtPoint(22, transform.position);

        }
    }
}