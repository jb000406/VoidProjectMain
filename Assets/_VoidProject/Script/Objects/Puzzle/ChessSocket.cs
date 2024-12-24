using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using VoidProject;

namespace VoidProject
{
    public class ChessSocket : MonoBehaviour
    {
        #region Variables
        public ChessBoardPuzzle chessBoard;

        public int socketIndex = -1;
        [SerializeField] private float soundVolume = 1f;

        public bool isInserted = false;
        public bool isCorrect = false;
        #endregion

        private void Start ()
        {
            //트리거 설정
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("충돌");

            Chess chess = other.GetComponent<Chess>();
            if (chess != null && !isInserted && !chess.isGrabbed)
            {
                if (chess.chessIndex == socketIndex)
                {
                    //사운드 재생
                    SoundManager.Instance.PlayClipAtPoint(21, transform.position, soundVolume);

                    //체스말 장착
                    Insert(other);

                    isCorrect = true;

                    chessBoard.isCorrect.RemoveAt(socketIndex);
                    chessBoard.isCorrect.Insert(socketIndex, isCorrect);

                    XRGrabInteractable interactable = other.GetComponent<XRGrabInteractable>();
                    interactable.enabled = false;
                }
                else
                {
                    Debug.Log("틀림");
                }
            }
        }

        private void Insert(Collider other)
        {
            isInserted = true;

            Debug.Log("장착");

            //퍼즐 아이템 위치, 방향
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;

            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }
}