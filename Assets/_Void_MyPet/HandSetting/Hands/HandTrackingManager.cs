using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VoidProject
{
    public class HandTrackingManager : MonoBehaviour
    {
        public static HandTrackingManager Instance { get; private set; }
        public GameObject originPosition;   // 원점 위치
        public Transform leftHandsModel;
        public Transform rightHandsModel;
        //public XRDirectInteractor[] directInteractors;
        //public XRRayInteractor[] rayInteractors;
        private XRHandSubsystem handSubsystem;

        private Dictionary<XRHandJointID, Transform> leftHand = new Dictionary<XRHandJointID, Transform>();
        private Dictionary<XRHandJointID, Transform> rightHand = new Dictionary<XRHandJointID, Transform>();
        private Dictionary<XRHandJointID, Pose> leftFingerObjects = new Dictionary<XRHandJointID, Pose>();
        private Dictionary<XRHandJointID, Pose> rightFingerObjects = new Dictionary<XRHandJointID, Pose>();
        [SerializeField] private float[] leftFingerDistanceToPalm;
        [SerializeField] private float[] rightFingerDistanceToPalm;
        [SerializeField] private float leftFingerDistancePinch = 0;
        [SerializeField] private float rightFingerDistancePinch = 0;
        // 임계값
        //private const float pinchThreshold = 0.03f; // 핀치 거리 (2cm)
        //private const float fistThreshold = 0.08f;  // Fist 거리 (10cm)
        [SerializeField] private float pinchThreshold = 0.03f;
        [SerializeField] private float fistThreshold = 0.08f;
        [SerializeField] private float leftPalmDownThreshold = 0.05f;
        [SerializeField] private float rightPalmDownThreshold = 0.05f;
        [SerializeField] private bool isLeftPalmDown;
        [SerializeField] private bool isRightPalmDown;
        [SerializeField] private Pose isLeftPalm;
        [SerializeField] private Pose isRightPalm;
        private bool hasHandRootModel = false;

        private static readonly Pose defaultPose = new Pose(Vector3.zero, Quaternion.identity);

        private readonly XRHandJointID[] requireJointID = {
            XRHandJointID.Wrist,
            XRHandJointID.Palm,
            XRHandJointID.ThumbMetacarpal, XRHandJointID.ThumbProximal, XRHandJointID.ThumbDistal, XRHandJointID.ThumbTip,
            XRHandJointID.IndexMetacarpal, XRHandJointID.IndexProximal, XRHandJointID.IndexIntermediate, XRHandJointID.IndexDistal, XRHandJointID.IndexTip,
            XRHandJointID.MiddleMetacarpal, XRHandJointID.MiddleProximal, XRHandJointID.MiddleIntermediate, XRHandJointID.MiddleDistal, XRHandJointID.MiddleTip,
            XRHandJointID.RingMetacarpal, XRHandJointID.RingProximal, XRHandJointID.RingIntermediate, XRHandJointID.RingDistal, XRHandJointID.RingTip,
            XRHandJointID.LittleMetacarpal, XRHandJointID.LittleProximal, XRHandJointID.LittleIntermediate, XRHandJointID.LittleDistal, XRHandJointID.LittleTip
        };

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            leftFingerDistanceToPalm = new float[5];
            rightFingerDistanceToPalm = new float[5];
            for (int i = 0; i < 5; i++) // ThumbTip부터 시작
            {
                leftFingerDistanceToPalm[i] = 0;
                rightFingerDistanceToPalm[i] = 0;
            }
        }

        private void Start()
        {
            StartCoroutine(InitializeHandSubsystem());
        }

        private IEnumerator InitializeHandSubsystem()
        {
            while (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.Log("XR Management 초기화 대기 중...");
                yield return null;
            }

            handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();

            if (handSubsystem == null)
            {
                Debug.LogError("XRHandSubsystem을 찾을 수 없습니다.");
            }
            else
            {
                Debug.Log("XRHandSubsystem 초기화 성공!");
                InitializeFingerObjects(leftHandsModel, leftHand, leftFingerObjects);
                InitializeFingerObjects(rightHandsModel, rightHand, rightFingerObjects);
            }
        }


        private void CheckHandSubsystem()
        {
            handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader?.GetLoadedSubsystem<XRHandSubsystem>();

            if (handSubsystem == null)
            {
                Debug.LogError("XRHandSubsystem이 로드되지 않았습니다.");
            }
            else
            {
                Debug.Log("XRHandSubsystem 초기화 성공!");
            }

            // 손가락 데이터 초기화
            InitializeFingerObjects(leftHandsModel, leftHand, leftFingerObjects);
            InitializeFingerObjects(rightHandsModel, rightHand, rightFingerObjects);
        }

        private void Update()
        {
            HandsUpdate();
        }

        private void LateUpdate()
        {
            HandsUpdate();
        }

        void HandsUpdate()
        {
            if (handSubsystem == null) return;

            UpdateHand(handSubsystem.leftHand, leftHand, leftFingerObjects);
            UpdateHand(handSubsystem.rightHand, rightHand, rightFingerObjects);
        }


        private void UpdateHand(XRHand hand, Dictionary<XRHandJointID, Transform> handTransform, Dictionary<XRHandJointID, Pose> fingerObjects)
        {
            if (!hand.isTracked) return;

            UpdateFingerObjects(hand, handTransform, fingerObjects);
        }

        private void InitializeFingerObjects(Transform handRoot, Dictionary<XRHandJointID, Transform> handTransform, Dictionary<XRHandJointID, Pose> fingerObjects)
        {
            foreach (var tipID in requireJointID)
            {
                //Transform jointTransform = FindJointTransform(handRoot, tipID);
                Transform jointTransform = null;
                if (jointTransform != null)
                {
                    hasHandRootModel = true;
                    handTransform[tipID] = jointTransform;
                }
                if (!fingerObjects.ContainsKey(tipID))
                {
                    fingerObjects[tipID] = defaultPose;
                }
            }
        }

        private Transform FindJointTransform(Transform root, XRHandJointID jointID)
        {
            string jointName = $"XRHand_{jointID}";
            foreach (Transform child in root.GetComponentsInChildren<Transform>())
            {
                if (child.name == jointName)
                    return child;
            }
            return null;
        }

        private bool IsFingerTip(XRHandJointID jointID)
        {
            return jointID == XRHandJointID.ThumbTip ||
                   jointID == XRHandJointID.IndexTip ||
                   jointID == XRHandJointID.MiddleTip ||
                   jointID == XRHandJointID.RingTip ||
                   jointID == XRHandJointID.LittleTip;
        }


        private void UpdateFingerObjects(XRHand hand, Dictionary<XRHandJointID, Transform> handTransform, Dictionary<XRHandJointID, Pose> fingerObjects)
        {
            if (!hand.isTracked) return;

            Pose palmPose = defaultPose;
            Pose thumbTipPose = defaultPose;
            Pose indexTipPose = defaultPose;
            bool hasPalm = false;

            int indexCount = 0;

            foreach (var jointID in requireJointID)
            {
                XRHandJoint joint = hand.GetJoint(jointID);

                if (joint.TryGetPose(out Pose jointPose))
                {
                    if (jointID == XRHandJointID.Palm)
                    {
                        palmPose = jointPose;
                        hasPalm = true;
                    }
                    else if (jointID == XRHandJointID.ThumbTip)
                    {
                        thumbTipPose = jointPose;
                    }
                    else if (jointID == XRHandJointID.IndexTip)
                    {
                        indexTipPose = jointPose;
                    }

                    if (hasHandRootModel && handTransform.ContainsKey(jointID))
                    {
                        handTransform[jointID].position = originPosition.transform.position + jointPose.position;
                        handTransform[jointID].rotation = originPosition.transform.rotation * jointPose.rotation;
                    }
                    if (hasPalm && IsFingerTip(jointID))
                    {
                        float distanceToPalm = Vector3.Distance(palmPose.position, jointPose.position);

                        if (hand == handSubsystem.leftHand)
                        {
                            leftFingerDistanceToPalm[indexCount] = distanceToPalm;
                        }
                        else
                        {
                            rightFingerDistanceToPalm[indexCount] = distanceToPalm;
                        }
                        fingerObjects[jointID] = jointPose;
                        indexCount++;
                    }
                }
            }

            // 핀치 거리 계산 (ThumbTip - IndexTip)
            float pinchDistance = Vector3.Distance(thumbTipPose.position, indexTipPose.position);
            if (hand == handSubsystem.leftHand)
            {
                leftFingerDistancePinch = pinchDistance;
                isLeftPalmDown = Vector3.Dot(palmPose.forward, Vector3.down) > leftPalmDownThreshold;
                isLeftPalm = palmPose;
                //if (!hasHandRootModel)
                //{
                //    leftHandsModel.position = originPosition.transform.position + palmPose.position;
                //    leftHandsModel.rotation = originPosition.transform.rotation * palmPose.rotation;
                //}
                //if (rayInteractors.Length > 0 && rayInteractors[0] != null)
                //{
                //    rayInteractors[0].transform.position = originPosition.transform.position + palmPose.position;
                //    rayInteractors[0].transform.rotation = originPosition.transform.rotation * palmPose.rotation;
                //    directInteractors[0].transform.position = originPosition.transform.position + palmPose.position;
                //    directInteractors[0].transform.rotation = originPosition.transform.rotation * palmPose.rotation;
                //}
            }
            else
            {
                rightFingerDistancePinch = pinchDistance;
                isRightPalmDown = Vector3.Dot(palmPose.forward, Vector3.down) > rightPalmDownThreshold;
                isRightPalm = palmPose;
                //if (!hasHandRootModel)
                //{
                //    rightHandsModel.position = originPosition.transform.position + palmPose.position;
                //    rightHandsModel.rotation = originPosition.transform.rotation * palmPose.rotation;
                //}
                //if (rayInteractors.Length > 0 && rayInteractors[1] != null)
                //{
                //    rayInteractors[1].transform.position = originPosition.transform.position + palmPose.position;
                //    rayInteractors[1].transform.rotation = originPosition.transform.rotation * palmPose.rotation;
                //    directInteractors[1].transform.position = originPosition.transform.position + palmPose.position;
                //    directInteractors[1].transform.rotation = originPosition.transform.rotation * palmPose.rotation;
                //}
            }
        }


        public bool IsPinching(bool isLeft)
        {
            if (!TrackingCheck(isLeft)) return false;
            float thumbToIndexDistance = isLeft ? leftFingerDistancePinch : rightFingerDistancePinch;
            return thumbToIndexDistance < pinchThreshold;
        }

        public bool IsFist(bool isLeft)
        {
            if (!TrackingCheck(isLeft)) return false;
            float[] fingerDistances = isLeft ? leftFingerDistanceToPalm : rightFingerDistanceToPalm;
            for (int i = 2; i < fingerDistances.Length; i++) 
            {
                if (fingerDistances[i] > fistThreshold)
                {
                    return false;
                }
            }
            return true;
        }


        public bool IsWalk(out Pose isLeft, out Pose isRight)
        {
            isLeft = defaultPose;
            isRight = defaultPose;
            if (!TrackingCheck(true) || !TrackingCheck(false)) return false;
            isLeft = isLeftPalm;
            isRight = isRightPalm;
            if(!IsFist(true) || !IsFist(false)) return false;
            return !isLeftPalmDown && !isRightPalmDown;
        }

        public static bool TrackingCheck(bool isLeft)
        {
            if (Instance.handSubsystem == null)
            {
                Debug.LogError("XRHandSubsystem이 로드되지 않았습니다.");
            }
            return isLeft ? Instance.handSubsystem.leftHand.isTracked : Instance.handSubsystem.rightHand.isTracked;
        }

        public static XRHand GetHand(bool isLeft)
        {
            return isLeft ? Instance.handSubsystem.leftHand : Instance.handSubsystem.rightHand;
        }

        public static XRHandSubsystem GetHandSubsystem() => Instance.handSubsystem;
    }
}
