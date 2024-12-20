using UnityEngine;
using UnityEngine.SceneManagement;

namespace VoidProject
{
    public class End : MonoBehaviour
    {
        public SceneFader fader;

        // 다른 객체와 충돌했을 때 호출되는 함수
        private void OnTriggerEnter(Collider other)
        {
            // 충돌한 객체의 태그가 targetColliderTag일 때만 씬을 전환
            if (other.CompareTag("Player"))
            {
                // 씬 전환
                fader.FadeTo(2);
            }
        }
    }
}