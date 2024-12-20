using System.Collections.Generic;
using UnityEngine;

namespace VoidProject
{

    public class HintSpawnManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform[] chessSpawnPoint;           // 스폰 포인트
        [SerializeField] private GameObject[] hintObj;                 // 단서 오브젝트 

        private List<GameObject> allHintObj = new List<GameObject>();  // 모든 체스 오브젝트
        private List<GameObject> availableHintObj = new List<GameObject>(); // 아직 사용되지 않은 체스 오브젝트
        private List<Transform> availableSpawnPoints = new List<Transform>(); // 남은 스폰 포인트
        private List<GameObject> spawnedHintObj = new List<GameObject>();    // 현재 스폰된 오브젝트들
        #endregion

        void Start()
        {
            InitializeChessObjects();
            SpawnChessWithProbability();
            SpawnRemainingChessObjects();
        }

        private void InitializeChessObjects()
        {
            // 모든 체스 오브젝트를 리스트에 등록
            allHintObj.AddRange(hintObj);
            availableHintObj.AddRange(hintObj);

            // 모든 스폰 포인트를 사용 가능한 리스트에 추가
            availableSpawnPoints.AddRange(chessSpawnPoint);
        }

        private void SpawnChessWithProbability()
        {
            foreach (Transform spawnPoint in chessSpawnPoint)
            {
                // 50% 확률로 스폰 여부 결정
                if (Random.value <= 0.5f && availableHintObj.Count > 0)
                {
                    // 랜덤하게 하나의 체스 오브젝트 선택
                    int randomIndex = Random.Range(0, availableHintObj.Count);
                    GameObject chessToSpawn = availableHintObj[randomIndex];

                    // 체스 오브젝트 스폰
                    SpawnChessObject(chessToSpawn, spawnPoint);

                    // 사용된 체스 오브젝트 제거
                    availableHintObj.RemoveAt(randomIndex);
                    availableSpawnPoints.Remove(spawnPoint);
                }
            }
        }

        private void SpawnRemainingChessObjects()
        {
            while (availableHintObj.Count > 0 && availableSpawnPoints.Count > 0)
            {
                // 첫 번째 남은 체스 오브젝트와 스폰 포인트 선택
                GameObject chessToSpawn = availableHintObj[0];
                Transform spawnPoint = availableSpawnPoints[0];

                // 체스 오브젝트 스폰
                SpawnChessObject(chessToSpawn, spawnPoint);

                // 사용된 오브젝트와 스폰 포인트 제거
                availableHintObj.RemoveAt(0);
                availableSpawnPoints.RemoveAt(0);
            }
        }

        private void SpawnChessObject(GameObject chessObject, Transform spawnPoint)
        {
            GameObject spawnedObject = Instantiate(chessObject, spawnPoint.position, Quaternion.identity);
            spawnedHintObj.Add(spawnedObject);
            Debug.Log($"스폰된 오브젝트: {chessObject.name} at {spawnPoint.name}");
        }
    }
}
