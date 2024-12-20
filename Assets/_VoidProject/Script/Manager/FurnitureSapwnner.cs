using HJ;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace VoidProject
{

    public class FurnitureSapwnner : MonoBehaviour
    {
        [Header("작은 항아리 오브젝트 스폰")]
        [SerializeField] private List<GameObject> randomSmallBreakObjectList = new List<GameObject>();
        [SerializeField] private Transform SmallBreakparentSpawner;
        [Header("디버그용 리스트")] // 생성된 오브젝트 리스트
        [SerializeField] private Transform[] SmallBreakSpawnerPoint;

        [Header("잠복형 오브젝트 스폰")]
        [SerializeField] private List<GameObject> randomAmbusherList = new List<GameObject>();
        [SerializeField] private Transform AmbusherparentSpawner;
        [Header("디버그용 리스트")] // 생성된 오브젝트 리스트
        [SerializeField] private Transform[] AmbusherSpawnerPoint;
        [SerializeField] private List<GameObject> spawnObject = new List<GameObject>();

        [SerializeField] private Transform player_Transform;
        private void Awake()
        {
            if (SmallBreakparentSpawner != null)
            {
                int breakableChild = SmallBreakparentSpawner.childCount;

                SmallBreakSpawnerPoint = new Transform[breakableChild];
                for (int i = 0; i < breakableChild; i++)
                {
                    SmallBreakSpawnerPoint[i] = SmallBreakparentSpawner.GetChild(i).transform;
                }
                RandomSmallObjSpawn();
            }

            if (AmbusherparentSpawner != null)
            {
                int monsterChild = AmbusherparentSpawner.childCount;

                AmbusherSpawnerPoint = new Transform[monsterChild];
                for (int i = 0; i < monsterChild; i++)
                {
                    AmbusherSpawnerPoint[i] = AmbusherparentSpawner.GetChild(i).transform;
                }
                RandomAmbusherObjSpawn();
            }

        }


        private void RandomSmallObjSpawn()
        {
            //int randomInt = Random.Range(0, parentSpawner.childCount);


            for (int i = 0; i < SmallBreakSpawnerPoint.Length; i++)
            {
                int randomBreakobjInt = Random.Range(0, randomSmallBreakObjectList.Count);
                RandomNumber();
                if (RandomNumber() % 3 == 0)
                {
                    var bobj = Instantiate(randomSmallBreakObjectList[randomBreakobjInt], SmallBreakSpawnerPoint[i].position, Quaternion.identity, SmallBreakSpawnerPoint[i]);
                    BreakableObject breakobj = bobj.GetComponent<BreakableObject>();
                    breakobj.player_Transform = player_Transform;
                    spawnObject.Add(bobj);
                }
            }
        }

        private void RandomAmbusherObjSpawn()
        {

            for (int i = 0; i < AmbusherSpawnerPoint.Length; i++)
            {
                int randomAmbusherobjInt = Random.Range(0, randomAmbusherList.Count);
                RandomNumber();
                if (RandomNumber() % 4 == 0)
                {
                    var cobj = Instantiate(randomAmbusherList[randomAmbusherobjInt], AmbusherSpawnerPoint[i].position, AmbusherSpawnerPoint[i].transform.rotation, AmbusherSpawnerPoint[i]);
                    cobj.transform.localScale = new Vector3(1f, 1f, 1f);
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