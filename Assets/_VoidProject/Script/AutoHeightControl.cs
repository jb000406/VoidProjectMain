using UnityEngine;

public class AutoHeightControl : MonoBehaviour
{
    [Header("카메라와 각 콜라이더등 자동 높이 및 중심 조정")]
    public CharacterController characterController; // 캐릭터 컨트롤러
    public Transform vrCamera; // VR 카메라 (HMD)

    // 상체와 하체의 충돌 영역
    public CapsuleCollider upperBodyCollider;
    //public CapsuleCollider lowerBodyCollider;

    // 최소 높이 제한
    public float minHeight = 0.5f;
    public float colliderOffset = 0.1f; // 충돌체 기본 오프셋

    void Update()
    {
        AutoAdjustHeightAndColliders();
    }

    /// <summary>
    /// 자동으로 캐릭터 컨트롤러와 콜라이더의 높이 및 중심 조정
    /// </summary>
    void AutoAdjustHeightAndColliders()
    {
        // 1. VR 카메라(HMD)의 높이 가져오기
        float headsetHeight = Mathf.Max(vrCamera.position.y, minHeight); // 최소 높이 보장

        // 2. 캐릭터 컨트롤러 조정
        characterController.height = headsetHeight; // 높이 설정
        characterController.center = new Vector3(0, headsetHeight / 2, 0); // 중심 설정

        // 3. 상체(전신) 콜라이더 조정
        if (upperBodyCollider != null)
        {
            upperBodyCollider.height = headsetHeight; // 높이 설정
            upperBodyCollider.center = new Vector3(0, headsetHeight / 2, 0); // 중심 설정
        }

        //// 4. 하체 콜라이더 조정
        //if (lowerBodyCollider != null)
        //{
        //    lowerBodyCollider.height = (headsetHeight / 2) - colliderOffset; // 하체는 절반
        //    lowerBodyCollider.center = new Vector3(0, headsetHeight / 4, 0); // 중심: 하단 1/4
        //}
    }
}
