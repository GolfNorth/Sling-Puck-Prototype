using System;
using UnityEngine;
using SlingPuck.Init;
using SlingPuck.Enum;

namespace SlingPuck.Behaviours
{
    public class RubberRopeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private BoardSide side;
        [SerializeField]
        private Transform startPoint;
        [SerializeField]
        private Transform endPoint;
        [SerializeField]
        private PolygonCollider2D polygonCollider;

        private PuckBehaviour _puck;
        private GameBoard _gameBoard;
        private int _ignoreLayer;
        private Vector2 _tangentA;
        private Vector2 _tangentB;
        private Vector3 _launchPosition;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector2 _originalPolygonPoint;

        private void Awake()
        {
            _gameBoard = GameObject.Find("SceneManager").GetComponent<SceneManager>().GameBoard;
            
            _ignoreLayer = LayerMask.NameToLayer("Puck Ignore");
            
            _startPosition = startPoint.position;
            _endPosition = endPoint.position;
            
            _originalPolygonPoint = new Vector2(
                0,
                side == BoardSide.Upper 
                    ? _gameBoard.RubberRope.PositionY
                    : - _gameBoard.RubberRope.PositionY);
            
            _launchPosition = _tangentA = _tangentB = _originalPolygonPoint;
        }

        private void Update()
        {
            if (_puck == null) return;

            var points = polygonCollider.points;
            var packPosition = _puck.Transform.position;
            
            _tangentA = points[0] = GetTangent(packPosition, _gameBoard.Puck.Radius, _startPosition);
            _tangentB = points[1] = GetTangent(packPosition, _gameBoard.Puck.Radius, _endPosition);

            polygonCollider.SetPath(0, points);
            
            _launchPosition = GetLaunchPosition(packPosition, _tangentA, _tangentB);
        }

        private void OnTriggerEnter2D(Collider2D puckCollider)
        {
            if (_puck != null) return;
            if (puckCollider.gameObject.layer != _ignoreLayer) return;
            
            _puck = puckCollider.gameObject.GetComponent<PuckBehaviour>();
            _puck.Releasing += PuckOnReleasing;
        }

        private void OnTriggerExit2D(Collider2D puckCollider)
        {
            if (_puck == null || _puck.GameObject != puckCollider.gameObject) return;
            
            _puck.Releasing -= PuckOnReleasing;
            _puck = null;
            
            var points = polygonCollider.points;
            
            _launchPosition = _tangentA = _tangentB = points[0] = points[1] = _originalPolygonPoint;

            polygonCollider.SetPath(0, points);
        }
        
        /// <summary>
        ///     Метод обработки события начала запуска шайбы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PuckOnReleasing(object sender, EventArgs e)
        {
            var force = GetForce();
            var direction = GetDirection(_puck.Transform.position, _launchPosition, force);

            _puck.Rigidbody2D.AddForce(direction, ForceMode2D.Impulse);
        }
        
        /// <summary>
        ///     Получение касательной к шайбе
        /// </summary>
        /// <param name="center">Центр шайбы</param>
        /// <param name="radius">Радиус шайбы</param>
        /// <param name="point">Точка в пространстве</param>
        /// <returns>Точка касательной к шайбе</returns>
        private Vector2 GetTangent(Vector2 center, float radius, Vector2 point)
        {
            point -= center;
            
            var p = point.magnitude;

            var a = radius * radius / p;    
            var q = radius * Mathf.Sqrt((p * p) - (radius * radius)) / p;
         
            var pN  = point / p;
            var pNp = new Vector2(-pN.y, pN.x);
            var va  = pN * a;

            var tangentA = va + pNp * q;
            var tangentB = va - pNp * q;
            
            var tangent = side == BoardSide.Upper
                ? tangentA.y > tangentB.y ? tangentA : tangentB
                : tangentA.y < tangentB.y ? tangentA : tangentB;
            
            tangent += center;
         
            return tangent;
        }
        
        private Vector3 GetDirection(Vector3 center, Vector3 launchPosition, float force)
        {
            return (center - launchPosition).normalized * force + launchPosition;
        }

        /// <summary>
        ///     Получение позиции отправной точки шайбы
        /// </summary>
        /// <param name="center">Центр шайбы</param>
        /// <param name="tangentA">Первая касательная к шайбе</param>
        /// <param name="tangentB">Вторая касательная к шайбе</param>
        /// <returns>Отправная точка шайбы</returns>
        private Vector3 GetLaunchPosition(Vector3 center, Vector3 tangentA, Vector3 tangentB)
        {
            tangentA -= center;
            tangentB -= center;

            return Vector3.Slerp(tangentA, tangentB, 0.5f) + center;
        }

        /// <summary>
        ///     Расчет силы ускорения для шайбы
        /// </summary>
        /// <returns>Сила ускорения</returns>
        private float GetForce()
        {
            var tension = Vector3.Distance(_startPosition, _tangentA)
                          + Vector3.Distance(_tangentA, _launchPosition)
                          + Vector3.Distance(_launchPosition, _tangentB)
                          + Vector3.Distance(_tangentB, _endPosition)
                          - Vector3.Distance(_startPosition, _endPosition);

            var force = tension > 0 ? _gameBoard.RubberRope.MaxForce * Mathf.Sqrt(tension / _gameBoard.RubberRope.MaxTension) : 0;
            
            return force;
        }
        
        public Vector2 TangentA => _tangentA;
        public Vector2 TangentB => _tangentB;
        public Vector3 LaunchPosition => _launchPosition;
    }
}
