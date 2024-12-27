using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using VoidProject;

public enum PetState
{
    Idle,
    Interact,
    Move,
    Indicate,
    Dissolve
}


public class MyPet_Indicater : MonoBehaviour
{
    [SerializeField] private float detectionRange = 2f; // 고양이가 완전히 나타나는 범위
    [SerializeField] private float interactionRange = 2f; // 플레이어와의 상호작용 범위
    [SerializeField] private float maxDissolveRange = 15f; // 고양이가 완전히 사라지는 거리
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Material[] ghostMaterial; // 고스트 쉐이더가 적용된 메터리얼
    private int currentWaypointIndex = 0;
    private float dissolveThreshold = 1f; // 1 = 완전히 사라짐, 0 = 완전히 보임
    private float fresnelPower = 0.0f; // 0 = 효과 없음. 최대 10 발광 강도

    private Animator animator;
    private PetState currentState = PetState.Dissolve;
    private bool isMoving = false; // 이동 중인지 확인하는 플래그

    private void Start()
    {
        ghostMaterial = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials;
        if (ghostMaterial == null)
        {
            Debug.LogError("Ghost Material이 설정되지 않았습니다!");
            enabled = false;
            return;
        }

        animator = GetComponent<Animator>();

        ghostMaterial[2].SetFloat("_FresnelPower", fresnelPower);
        ghostMaterial[1].SetFloat("_DissolveThreshold", dissolveThreshold);
        animator.SetBool("IsLieSleep", true);
        ChangeState(PetState.Dissolve);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Player_Transform.position);
        UpdateDissolve(distanceToPlayer);
        switch (currentState)
        {
            case PetState.Dissolve:
                currentState = PetState.Idle;
                break;
            case PetState.Idle:
                HandleIdleState(distanceToPlayer);
                break;
            case PetState.Interact:
                break;
            case PetState.Move:
                HandleMoveState();
                break;
            case PetState.Indicate:
                HandleIndicate();
                break;
        }
    }

    private void ChangeState(PetState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case PetState.Dissolve:
                currentState = PetState.Idle;
                break;
            case PetState.Idle:
                if (animator != null)
                {
                    GameManager.IsInteractiveWithPet = false;
                    GameManager.ToggleCanvas(false);
                    animator.SetFloat("MoveX", 0);
                    animator.SetBool("IsLieSleep", true);
                    if (currentWaypointIndex < 4)
                    {
                        GameManager.Player_Transform.GetChild(1).gameObject.SetActive(true);
                    }
                    // navAgent.enabled = false;
                }
                break;
            case PetState.Interact:
                InteractWithPlayer();
                break;
            case PetState.Move:
                MoveToNextWaypoint();
                break;
            case PetState.Indicate:
                SetleIndicate();
                break;
        }
    }

    private void HandleIdleState(float distanceToPlayer)
    {
        if (distanceToPlayer < interactionRange)
        {
            ChangeState(PetState.Interact);
        }
        else if (distanceToPlayer >= maxDissolveRange)
        {
            ChangeState(PetState.Dissolve);
        }
    }

    private void HandleIndicate()
    {

    }

    private void SetleIndicate()
    {

    }

    private void HandleMoveState()
    {
        if (!isMoving) // 이동 중이 아니면 이동 시작
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        if (isMoving) return; // 이미 이동 중이면 중복 호출 방지

        currentWaypointIndex = (currentWaypointIndex + 1) % GameManager.SequencePointList.Count;
        Vector3 nextWaypoint = GameManager.SequencePointList[currentWaypointIndex];

        if (nextWaypoint == Vector3.zero)
        {
            Debug.LogError("다음 Waypoint가 null입니다. waypoints 배열을 확인하세요.");
            return;
        }

        isMoving = true; // 이동 상태 플래그 활성화
        StartCoroutine(MoveTowards(nextWaypoint));
    }

    private IEnumerator MoveTowards(Vector3 targetPosition)
    {
        Vector3 target = AdjustHeightToTerrain(targetPosition);
        animator.SetFloat("MoveX", 2);

        if (GameManager.IsInteractiveWithPet)
        {
            DisplayText();
        }

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            Vector3 direction = (target - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        //while (Vector3.Distance(transform.position, GameManager.Player_Transform.position) > 0.1f)
        //{
        //    Vector3 directionToPlayer = (GameManager.Player_Transform.position - transform.position).normalized;

        //    if (directionToPlayer != Vector3.zero)
        //    {
        Vector3 directionToPlayer = (GameManager.Player_Transform.position - transform.position).normalized;
        Quaternion targetRotation2 = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = targetRotation2;
        //    }

        //    yield return null;
        //}

        isMoving = false;
        animator.SetFloat("MoveX", 0);
        ChangeState(PetState.Idle);
    }


    private Vector3 AdjustHeightToTerrain(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 0.5f, Vector3.down, out hit, Mathf.Infinity))
        {
            position.y = hit.point.y;
        }
        else
        {
            Debug.LogWarning("Raycast로 지형 높이를 찾을 수 없습니다. 기본 값을 사용합니다.");
        }
        return position;
    }



    private void InteractWithPlayer()
    {
        if (animator != null)
        {
            if (currentWaypointIndex < 4)
            {
                GameManager.Player_Transform.GetChild(1).gameObject.SetActive(false);
            }
            animator.SetBool("IsLieSleep", false);
            //animator.SetBool("IsEat", true);
            animator.SetBool("IsSit", true);
        }

        StartCoroutine(WaitTextReadEnd());
        // 먹이줬을때의 행동일때 사용
        //StartCoroutine(WaitForAnimationAndMove());
    }

    private IEnumerator WaitTextReadEnd()
    {
        yield return new WaitForSeconds(4f);
        if (animator != null)
        {
            animator.SetBool("IsSit", false);
        }
        GameManager.IsInteractiveWithPet = true;
        DisplayText();
        ChangeState(PetState.Move);
    }
    private IEnumerator WaitForAnimationAndMove()
    {
        yield return new WaitUntil(() => IsAnimationFinished("IsEat"));

        if (animator != null)
        {
            animator.SetBool("IsEat", false);
        }
        ChangeState(PetState.Move);
    }

    private bool IsAnimationFinished(string parameterName)
    {
        if (animator == null) return true;

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        return parameterName == "IsEat" && currentState.IsName("Arm_Cat|EatDrink_end") && currentState.normalizedTime >= 0.95f;
    }

    private void UpdateDissolve(float distanceToPlayer)
    {
        float dissolveValue = Mathf.InverseLerp(detectionRange, maxDissolveRange, distanceToPlayer);
        dissolveThreshold = Mathf.Clamp(dissolveValue, 0f, 1f);
        fresnelPower = 0.1f - (dissolveThreshold * 2f);
        ghostMaterial[1].SetFloat("_DissolveThreshold", dissolveThreshold);
        ghostMaterial[2].SetFloat("_FresnelPower", fresnelPower);
    }

    private void DisplayText()
    {
        string paragraph = GameManager.SequenceText[currentWaypointIndex];
        GameManager.DisplayToolTipText(paragraph);
    }
}
