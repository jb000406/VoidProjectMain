using System.Collections.Generic;
using UnityEngine;

namespace VoidProject
{
    public class Candle : MonoBehaviour
    {
        #region Variables
        public GameObject effect;
        public int igniteOrder;

        private Match match;
        public bool isLit = false;
        #endregion

        private void OnCollisionEnter(Collision collision)
        {
            match = collision.transform.GetComponent<Match>();

            if (match == null)
                return;

            if (collision.transform.CompareTag("Match") && match.isIgnition)
            {
                Ignite();
            }
        }

        //점화
        public void Ignite()
        {
            if (!isLit)
            {
                // 이펙트 실행
                effect.SetActive(true);

                isLit = true; //점화 상태

                //퍼즐 매니저
                CandlePuzzleManager puzzleManager = FindFirstObjectByType<CandlePuzzleManager>();
                puzzleManager.AttemptIgnite(this);
            }
        }

        // 끄기
        public void Extinguish()
        {
            if (isLit)
            {
                effect.SetActive(false);

                isLit = false;
            }
        }
    }
}