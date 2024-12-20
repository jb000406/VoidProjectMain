using UnityEngine;
using UnityEngine.AI;


namespace VoidProject
{
    public enum EnemyState
    {
        Roaming,
        Chasing,
        Attacking,
        Howling,
        Idle
    }

    public class ChaseMonsterController : MonoBehaviour
    {
        #region Variables
        [Header("NPC 설정")]
        [SerializeField] private float raderRadius = 5f;     // NPC의 감지 범위
        [SerializeField] private float moveSpeed = 2f;      // NPC 이동 속도
        [SerializeField] private float chaseSpeed = 4.5f;      // NPC 이동 속도
        [SerializeField] private float attackDistance = 1.5f; // 공격 거리
        [SerializeField] private float stopDistance = 3f; // 멈추는 거리
        [SerializeField] private float roamingBackTime = 4f; // 플레이어를 놓치고 가만히 있는 시간

        [Header("시야 설정")]
        [SerializeField] private float detectionRange = 10f; // 감지 범위
        [SerializeField] private float fieldOfView = 60f;    // 시야각 (각도)
        [SerializeField] private Transform eyePosition;      // 레이캐스트 시작 위치 (몬스터의 눈 위치)

        private Animator npc_Animator;                     // NPC의 Animator
        private SphereCollider npc_SphereCollider;         // 감지용 SphereCollider
        private Transform player_Transform;                // 플레이어 Transform
        private NavMeshAgent agent;

        public bool IsAttack { get; private set; }         // 공격 상태
        public bool IsMove { get; private set; }           // 이동 상태
        public bool IsRoaming { get; private set; }        // 순찰 상태
        public bool IsFind { get; private set; }           // 멈춤 상태
        public bool IsLost { get; private set; }           // 놓침 상태
        private bool isDetected = false;    //감지 상태
        private bool isRoaming = true;     //순찰 상태
        private bool isTriggered = false;   //트리거

        //페트롤
        [SerializeField] private Transform waypointParent;  // 웨이포인트 부모 오브젝트
        private Transform[] wayPoints;                       // 웨이포인트 배열
        private int nowWayPoint = 0;

        #endregion

        private void OnEnable()
        {
            npc_Animator = transform.GetChild(0).GetComponent<Animator>();
            npc_SphereCollider = GetComponent<SphereCollider>();
            agent = GetComponent<NavMeshAgent>();

            // 감지 범위를 설정
            npc_SphereCollider.radius = raderRadius;

            // 플레이어 참조 초기화
            player_Transform = null;
            // 웨이포인트 초기화
            if (waypointParent != null)
            {
                wayPoints = new Transform[waypointParent.childCount];
                for (int i = 0; i < waypointParent.childCount; i++)
                {
                    wayPoints[i] = waypointParent.GetChild(i);
                }
            }
        }

        private void Update()
        {
            DetectPlayer();

            // 플레이어가 감지가 되었을때
            if (player_Transform != null)
            {
                if (!isTriggered && !isDetected)     //플레이어를 마지막으로 본 위치로 이동
                {
                    CheckArrivalAtLastKnownPosition();
                    return;
                }
                
                // 플레이어와의 거리
                float distanceToPlayer = Vector3.Distance(transform.position, player_Transform.position);

                
                    // 기본적으로 접근
                    Debug.Log("기본 접근 상태");
                    ApproachToNear(player_Transform);


                    if (distanceToPlayer <= attackDistance)
                    {
                        // 공격 거리 안에 있다면 공격
                        Debug.Log("플레이어 공격");
                        AttackTarget(player_Transform);
                    }
                
                return;
            }

            // 플레이어가 감지가 안 되었을때 다음 웨이 포인트로 이동
            if (isRoaming)
            {
                Roaming();
                if (agent.remainingDistance < stopDistance)
                {
                    GoNextPoint();
                }
            }
        }

        private void GoNextPoint()
        {
            nowWayPoint++;
            if (nowWayPoint >= wayPoints.Length)
            {
                nowWayPoint = 0;
            }
            agent.SetDestination(wayPoints[nowWayPoint].position);
        }

        // 상태 설정
        public void SetState(bool isAttack, bool isRoaming, bool isFind, bool isLost)
        {
            // 상태 값 설정
            IsAttack = isAttack;
            IsRoaming = isRoaming;
            IsFind = isFind;
            IsLost = isLost;
            // 애니메이터에 상태 값 반영
            npc_Animator.SetBool("IsAttack", IsAttack);
            npc_Animator.SetBool("IsRoaming", IsRoaming);
            npc_Animator.SetBool("IsFind", IsFind);
            npc_Animator.SetBool("IsLost", IsLost);
        }

        // 순찰 상태
        private void Roaming()
        {
            SetState(false, true, false, false);
            agent.speed = moveSpeed;
            isRoaming = true;
        }

        // 플레이어를 향해 이동
        private void ApproachToNear(Transform target)
        {
            SetState(false, false, true, false);

            agent.speed = chaseSpeed;

            agent.SetDestination(target.position);
        }

        private void CheckArrivalAtLastKnownPosition()
        {
            if (!agent.pathPending && agent.remainingDistance <= stopDistance)
            {
                Debug.Log("마지막 위치 도착");
                SetState(false, false, false, true);
                // 3초 후 순찰 상태로 전환
                Invoke("Roaming", roamingBackTime);
                player_Transform = null;
            }
        }

        // 플레이어를 공격(아직 이동은 진행중)
        private void AttackTarget(Transform target)
        {
            SetState(true, false, true, false);
            agent.SetDestination(transform.position);
            //SoundManager.Instance.PlayClipAtPoint(1, transform.position, 1f);
            Vector3 direction = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }

        // 플레이어가 감지 영역에 들어올 때
        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                Debug.Log("플레이어 감지" + other.name);
                player_Transform = other.transform; // 플레이어 참조
                isTriggered = true;
                isRoaming = false;
            }
        }

        // 플레이어가 감지 영역에서 나갈 때
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("플레이어 영역에서 나감");
                isTriggered = false;
            }
        }

        private void DetectPlayer()
        {
            // OverlapSphere로 감지 영역 내의 모든 콜라이더 가져오기
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    // 플레이어와 몬스터 간의 방향 계산
                    Vector3 directionToPlayer = (collider.transform.position - eyePosition.position).normalized;
                    float angle = Vector3.Angle(transform.forward, directionToPlayer);

                    // 시야각 검사
                    if (angle < fieldOfView / 2)
                    {
                        if (Physics.Raycast(eyePosition.position, directionToPlayer, out RaycastHit hit, detectionRange))
                        {
                            if (hit.collider.CompareTag("Player"))
                            {
                                Debug.Log("플레이어 시야감지!");
                                player_Transform = hit.transform;
                                isDetected = true;
                                isRoaming = false;
                                return;
                            }
                        }
                    }
                }
            }

            isDetected = false;
        }

        private void OnDrawGizmos()
        {
            // 시야각 및 감지 범위 표시
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * detectionRange;
            Vector3 rightBoundary = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * detectionRange;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, raderRadius);
        }


    }

}