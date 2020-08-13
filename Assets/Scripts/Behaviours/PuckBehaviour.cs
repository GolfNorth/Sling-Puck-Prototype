using System;
using UnityEngine;
using SlingPuck.Enum;

namespace SlingPuck.Behaviours
{
    public class PuckBehaviour : MonoBehaviour
    {
        public event EventHandler Releasing;
        public event EventHandler Released;

        private bool _inHand;
        private int _puckLayer;
        private int _ignoreLayer;
        private Rigidbody2D _rigidbody;
        private TargetJoint2D _targetJoint;
        private Transform _playerTransform;

        private void Awake()
        {
            _inHand = false;
            _puckLayer = LayerMask.NameToLayer("Puck");
            _ignoreLayer = LayerMask.NameToLayer("Puck Ignore");
            _targetJoint = GetComponent<TargetJoint2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!_inHand) return;
            
            var playerPosition = _playerTransform.position;

            _targetJoint.target = new Vector2(playerPosition.x, playerPosition.y);

        }

        /// <summary>
        ///     Схватить шайбу
        /// </summary>
        /// <param name="playerTransform">Трансформ игрока схватившего шайбу</param>
        /// <returns>Удалось ли выполнить действие</returns>
        public bool Grab(Transform playerTransform)
        {
            if (_inHand) return false;
            
            _inHand = true;
            _playerTransform = playerTransform;
            _targetJoint.enabled = true;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            gameObject.layer = _ignoreLayer;

            return true;
        }

        /// <summary>
        ///     Отпустить шайбу
        /// </summary>
        public void Release()
        {
            Releasing?.Invoke(null, null);
        
            _inHand = false;
            _playerTransform = null;
            _targetJoint.enabled = false;
            _rigidbody.constraints = RigidbodyConstraints2D.None;

            gameObject.layer = _puckLayer;

            Released?.Invoke(null, null);
        }

        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
        public Rigidbody2D Rigidbody2D => _rigidbody;
        public BoardSide Side => transform.position.y >= 0 ? BoardSide.Upper : BoardSide.Lower;
    }
}