using UnityEngine;
using SlingPuck.Init;
using SlingPuck.Enum;

namespace SlingPuck
{
    public class SceneManager : MonoBehaviour
    {
        public Difficult difficult;
        [Header("Rubber Rope")]
        [SerializeField]
        private Transform rubberRopesParent;
        [SerializeField]
        private GameObject lowerRubberRopePrefab;
        [SerializeField]
        private GameObject upperRubberRopePrefab;
        [Header("Crossbar")]
        [SerializeField]
        private GameObject crossbarColliderEasy;
        [SerializeField]
        private GameObject crossbarColliderMiddle;
        [SerializeField]
        private GameObject crossbarColliderHard;
        [SerializeField]
        private GameObject crossbarMeshEasy;
        [SerializeField]
        private GameObject crossbarMeshMiddle;
        [SerializeField]
        private GameObject crossbarMeshHard;
        [Header("Puck")]
        [SerializeField]
        private Transform pucksParent;
        [SerializeField]
        private GameObject puckPrefab;

        private GameBoard _gameBoard;
        private GameObject[] _pucks;

        private void Awake()
        {
            _gameBoard = new GameBoard(difficult, crossbarColliderEasy, crossbarColliderMiddle, crossbarColliderHard, crossbarMeshEasy, crossbarMeshMiddle, crossbarMeshHard);
            
            Instantiate(upperRubberRopePrefab, rubberRopesParent);
            Instantiate(lowerRubberRopePrefab, rubberRopesParent);

            _pucks = new GameObject[10];
        }

        public GameBoard GameBoard => _gameBoard;
    }
}
