using UnityEngine;

namespace VoidProject
{
    public class EnemyAttackAnimation : MonoBehaviour
    {
        [Header("Enemy Attack Animation")]
        private Animator animator;
        [SerializeField] private float attackDamage = 10f;       //공격 데미지
        private void Start()
        {
            //참조
            animator = GetComponent<Animator>();

        }
        private void Attack()
        {
            Debug.Log("플레이어에게 데미지를 준다");
            // 플레이어에게 데미지
            if (GameManager.Player_Transform != null)
            {
                IDamageable damageable = GameManager.Player_Transform.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);
                }
            }
        }
    }
}
