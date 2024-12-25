using UnityEngine;

public class IdleStateMachine : StateMachineBehaviour
{
    public int maxRandomCount = 1;
    private int previousLoop = 0;

    // 상태가 진입(Enter)될 때 호출
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 랜덤으로 0 ~ 7의 값을 Animator 파라미터에 설정
        int randomIdle = Random.Range(0, maxRandomCount);
        animator.SetInteger("RandomIdle", randomIdle);
    }

    // 상태가 업데이트될 때 호출
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 현재 루프 횟수 계산
        int currentLoop = Mathf.FloorToInt(stateInfo.normalizedTime);

        // 현재 루프에서만 동작하도록 설정
        if (currentLoop > 0 && currentLoop != previousLoop)
        {
            int randomIdle = Random.Range(0, maxRandomCount);
            animator.SetInteger("RandomIdle", randomIdle);
            previousLoop = currentLoop; // 현재 루프 저장
        }
    }

    // 상태에서 나갈 때 호출
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Exit 상태로 전환
        animator.SetInteger("RandomIdle", 0);
    }
}