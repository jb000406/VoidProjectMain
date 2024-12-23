using System.Collections.Generic;
using UnityEngine;

namespace VoidProject
{
    public class ChessSpawnManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform[] chessSpawnPoint;           // 스폰 포인트
        [SerializeField] private GameObject[] chessObj;                 // 체스 오브젝트 

        private List<GameObject> allChessObj = new List<GameObject>();  // 모든 체스 오브젝트
        private List<GameObject> availableChessObj = new List<GameObject>(); // 아직 사용되지 않은 체스 오브젝트
        private List<Transform> availableSpawnPoints = new List<Transform>(); // 남은 스폰 포인트
        private List<GameObject> spawnedChessObj = new List<GameObject>();    // 현재 스폰된 오브젝트들
        #endregion

        void Start()
        {
            InitializeChessObjects();
            SpawnChessWithProbability();
            SpawnRemainingChessObjects();
        }

        /// <summary>
        /// 체스 오브젝트 초기화
        /// </summary>
        private void InitializeChessObjects()
        {
            // 모든 체스 오브젝트를 리스트에 등록
            allChessObj.AddRange(chessObj);
            availableChessObj.AddRange(chessObj);

            // 모든 스폰 포인트를 사용 가능한 리스트에 추가
            availableSpawnPoints.AddRange(chessSpawnPoint);
        }

        /// <summary>
        /// 스폰 포인트에 50% 확률로 체스 오브젝트를 배치
        /// </summary>
        private void SpawnChessWithProbability()
        {
            foreach (Transform spawnPoint in chessSpawnPoint)
            {
                // 50% 확률로 스폰 여부 결정
                if (Random.value <= 0.5f && availableChessObj.Count > 0)
                {
                    // 랜덤하게 하나의 체스 오브젝트 선택
                    int randomIndex = Random.Range(0, availableChessObj.Count);
                    GameObject chessToSpawn = availableChessObj[randomIndex];

                    // 체스 오브젝트 스폰
                    SpawnChessObject(chessToSpawn, spawnPoint);

                    // 사용된 체스 오브젝트 제거
                    availableChessObj.RemoveAt(randomIndex);
                    availableSpawnPoints.Remove(spawnPoint);
                }
            }
        }

        /// <summary>
        /// 남은 체스 오브젝트들을 남은 스폰 포인트에 배치
        /// </summary>
        private void SpawnRemainingChessObjects()
        {
            while (availableChessObj.Count > 0 && availableSpawnPoints.Count > 0)
            {
                // 첫 번째 남은 체스 오브젝트와 스폰 포인트 선택
                GameObject chessToSpawn = availableChessObj[0];
                Transform spawnPoint = availableSpawnPoints[0];

                // 체스 오브젝트 스폰
                SpawnChessObject(chessToSpawn, spawnPoint);

                // 사용된 오브젝트와 스폰 포인트 제거
                availableChessObj.RemoveAt(0);
                availableSpawnPoints.RemoveAt(0);
            }
        }

        /// <summary>
        /// 체스 오브젝트를 스폰하는 메서드
        /// </summary>
        /// <param name="chessObject">스폰할 체스 오브젝트</param>
        /// <param name="spawnPoint">스폰 위치</param>
        private void SpawnChessObject(GameObject chessObject, Transform spawnPoint)
        {
            GameObject spawnedObject = Instantiate(chessObject, spawnPoint.position, Quaternion.identity);
            spawnedChessObj.Add(spawnedObject);
            Debug.Log($"스폰된 오브젝트: {chessObject.name} at {spawnPoint.name}");
        }
    }
}