using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using VoidProject;
using static UnityEngine.ParticleSystem;

namespace VoidProject
{
    public class Match : MonoBehaviour
    {
        #region Variables
        private XRGrabInteractable interactable;
        private Rigidbody rb;
        private MeshRenderer meshRenderer;

        public ParticleSystem flame;
        public ParticleSystem smoke;
        public GameObject effect;
        public GameObject burntMatch;

        public float ignitionVelocity = 1f;
        public bool isIgnition = false;
        [SerializeField] private float burntDelay = 10f;
        [SerializeField] private float soundVolume = 1f;

        //public AudioSource audioSource;
        #endregion

        private void Start()
        {
            interactable = GetComponent<XRGrabInteractable>();
            rb = GetComponent<Rigidbody>();
            meshRenderer = GetComponent<MeshRenderer>();

            //이벤트 함수 등록
            interactable.selectEntered.AddListener(OnGrab);
            interactable.selectExited.AddListener(OnRelease);
        }

        //점화
        public IEnumerator Ignition()
        {
            Debug.Log("점화");

            //이펙트 플레이
            flame.Play();
            smoke.Play();

            SoundManager.Instance.PlayClipAtPoint(13, transform.position, soundVolume);

            //점화 가능
            isIgnition = true;

            yield return new WaitForSeconds(burntDelay);

            //메쉬, 이펙트 종료
            meshRenderer.enabled = false;
            effect.SetActive(false);

            //탄 성냥
            burntMatch.SetActive(true);

            //점화 불가능
            isIgnition = false;
        }

        public void OnGrab(SelectEnterEventArgs args)
        {
            rb.isKinematic = false;
        }

        private void OnRelease(SelectExitEventArgs args)
        {
            rb.isKinematic = false;
        }
    }
}