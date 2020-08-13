using SlingPuck.Init;
using UnityEngine;
using UnityEngine.InputSystem;
using SlingPuck.Enum;

namespace SlingPuck.Behaviours
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private BoardSide side;
        [SerializeField]
        private new Camera camera;
        [SerializeField]
        private bool lockY;
        [SerializeField]
        private InputAction grabAction;
        [SerializeField]
        private InputAction moveAction;
        
        private PuckBehaviour _puck = null;
        private GameBoard _gameBoard;
        private float _lastPositionY = 0;

        private void Start()
        { 
            _gameBoard = GameObject.Find("SceneManager").GetComponent<SceneManager>().GameBoard;
            
            grabAction.started += GrabActionOnstarted;
            grabAction.canceled += GrabActionOncanceled;
            
            moveAction.performed += MoveActionOnperformed;
        }

        private void OnEnable()
        {
            grabAction.Enable();
            moveAction.Enable();
        }

        private void OnDisable()
        {
            grabAction.Disable();
            moveAction.Disable();
        }

        private void MoveActionOnperformed(InputAction.CallbackContext obj)
        {
            var test = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (Physics.Raycast(camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit, Mathf.Infinity, LayerMask.GetMask("Field")))
            {
                var newPosition = new Vector3(hit.point.x, hit.point.y, 0);
                
                _gameBoard.StayAtField(side, ref newPosition);

                if (_puck != null && lockY) LockY(ref newPosition, ref _lastPositionY);

                transform.position = newPosition;
            }
        }

        private void GrabActionOncanceled(InputAction.CallbackContext obj)
        {
            if (_puck == null) return;
            
            _puck.Release();
            _puck = null;
        }

        private void GrabActionOnstarted(InputAction.CallbackContext obj)
        {
            if (_puck != null) return;

            if (Physics.Raycast(camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit, Mathf.Infinity, LayerMask.GetMask("Puck")))
            {
                var puckGameObject = hit.collider.transform.parent.gameObject;
                _puck = puckGameObject.GetComponent<PuckBehaviour>();

                if (_puck.Side != side) return;
                
                if (!_puck.Grab(transform)) return;

                _lastPositionY = puckGameObject.transform.position.y;
            }
        }
        
        private void LockY(ref Vector3 point, ref float lastY)
        {
            lastY = Mathf.Abs(point.y) < Mathf.Abs(_gameBoard.RubberRope.PositionY - _gameBoard.Puck.Radius)
                ? Mathf.Abs(point.y) < Mathf.Abs(lastY) ? lastY : point.y
                : point.y;

            point.y = lastY;
        }
    }
}
