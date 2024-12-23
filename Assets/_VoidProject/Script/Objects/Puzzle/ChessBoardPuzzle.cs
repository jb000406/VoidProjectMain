using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


namespace VoidProject
{
    public class ChessBoardPuzzle : MonoBehaviour
    {
        #region Variables
        public Transform canMovePieces;
        public Transform socket;

        [SerializeField] private List<Chess> chesses;
        [SerializeField] private List<ChessSocket> chessSocket;
        
        private int chessIndex = 0;
        private int socketIndex = 0;

        public List<bool> isCorrect;
        private bool isCompleted = false;

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject keyChest;
        #endregion

        private void Start()
        {
            //소켓, 장기말 초기화
            Initial();

            //장기말, 소켓을 리스트에 추가
            chesses.AddRange(canMovePieces.GetComponentsInChildren<Chess>());
            chessSocket.AddRange(socket.GetComponentsInChildren<ChessSocket>());

            //장기말, 소켓에 인덱스 설정(0~7)
            foreach (var c in chesses)
            {
                c.chessIndex = chessIndex;
                chessIndex++;
            }

            foreach(var s in chessSocket)
            {
                s.socketIndex = socketIndex;
                socketIndex++;
            }

            //isCorrect, isInserted 초기화
            for(int i = 0; i < chessSocket.Count; i++)
            {
                isCorrect.Add(false);
            }
        }

        private void Update()
        {
            if(IsAllCorrect())
            {
                if(!isCompleted)
                {
                    Completed();
                    isCompleted = true;
                }
            }
        }

        private void Completed()
        {
            Debug.Log("퍼즐 완성");
            Instantiate(keyChest, spawnPoint.position, Quaternion.identity);
        }

        //모든 소켓이 맞으면 true
        private bool IsAllCorrect()
        {
            return isCorrect.All(x => x);
        }

        //초기화
        private void Initial()
        {
            Debug.Log("초기화");

            //소켓 값 초기화
            foreach (var s in chessSocket)
            {
                s.isInserted = false;
                s.isCorrect = false;
            }

            foreach (var c in chesses)
            {
                c.transform.position = c.initialPosition;

                Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
                rb.isKinematic = false;
            }
        }

        private void InitializeList()
        {
            for (int i = 0; i < chessSocket.Count; i++)
            {
                isCorrect[i] = false;
            }
        }
    }
}