using HJ;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace VoidProject
{

    public class Sapwnner : MonoBehaviour
    {
        [Header("항아리 오브젝트 스폰")]
        [SerializeField] private List<GameObject> randomBreakableObjectList = new List<GameObject>();
        [SerializeField] private Transform BreakableparentSpawner;
        [Header("디버그용 리스트")] // 생성된 오브젝트 리스트
        [SerializeField] private Transform[] breakableSpawnerPoint;

        [Header("상자 오브젝트 스폰")]
        [SerializeField] private List<GameObject> randomCheswtObjectList = new List<GameObject>();
        [SerializeField] private Transform ChestparentSpawner;
        [Header("디버그용 리스트")] // 생성된 오브젝트 리스트
        [SerializeField] private Transform[] ChestSpawnerPoint;
        [SerializeField] private List<GameObject> spawnObject = new List<GameObject>();

        [SerializeField] private Transform player_Transform;
        private GameObject[] spawnPoints;      // 스폰 포인트 배열 -- 임시
        public GameObject key;

        private void Awake()
        {
            int breakableChild = BreakableparentSpawner.childCount;
            int chestChild = ChestparentSpawner.childCount;

            breakableSpawnerPoint = new Transform[breakableChild];
            for (int i = 0; i < breakableChild; i++)
            {
                breakableSpawnerPoint[i] = BreakableparentSpawner.GetChild(i).transform;
            }

            ChestSpawnerPoint = new Transform[chestChild];
            for (int i = 0; i < chestChild; i++)
            {
                ChestSpawnerPoint[i] = ChestparentSpawner.GetChild(i).transform;
            }

            RandomSpawn();
        }

        private void Start()
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("KeySpawnPoint");

            if (spawnPoints.Length > 0)
            {
                int randomSpawnNumber = Random.Range(0, spawnPoints.Length);

                Instantiate(key, spawnPoints[randomSpawnNumber].transform.GetChild(0).position, Quaternion.identity);

            }
        }

        private void RandomSpawn()
        {
            //int randomInt = Random.Range(0, parentSpawner.childCount);


            for (int i = 0; i < breakableSpawnerPoint.Length; i++)
            {
                int randomBreakobjInt = Random.Range(0, randomBreakableObjectList.Count);
                RandomNumber();
                if (RandomNumber() % 3 == 0)
                {
                    var bobj = Instantiate(randomBreakableObjectList[randomBreakobjInt], breakableSpawnerPoint[i].position, Quaternion.identity, breakableSpawnerPoint[i]);
                    BreakableObject breakobj = bobj.GetComponent<BreakableObject>();
                    breakobj.player_Transform = player_Transform;
                    spawnObject.Add(bobj);
                }
            }

            for (int i = 0; i < ChestSpawnerPoint.Length; i++)
            {
                int randomChestobjInt = Random.Range(0, randomCheswtObjectList.Count);
                RandomNumber();
                if (RandomNumber() % 2 == 0)
                {
                    var cobj = Instantiate(randomCheswtObjectList[randomChestobjInt], ChestSpawnerPoint[i].position, ChestSpawnerPoint[i].transform.rotation, ChestSpawnerPoint[i]);
                    //BreakableObject breakobj = cobj.GetComponent<BreakableObject>();
                    spawnObject.Add(cobj);
                }
            }
        }

        /* private void RandomKeySpawn()
         {
             int randomobjInt = Random.Range(0, spawnObject.Count);
             GameObject aa = spawnObject[randomobjInt].gameObject;
         }*/

        int RandomNumber()
        {
            return Random.Range(0, 100);
        }

    }
}