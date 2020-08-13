using UnityEngine;
using SlingPuck.Enum;

namespace SlingPuck.Init
{
    public class GameBoard
    {
        private readonly Vector3 _boardSize = new Vector2(33.75f, 60f);
        private readonly float _borderThickness = 2f;
        private readonly Vector2 _fieldSize;
        
        private readonly Crossbar _crossbar;
        private readonly Puck _puck;
        private readonly RubberRope _rubberRope;

        public GameBoard(Difficult difficult, GameObject colliderEasy, GameObject colliderMiddle, GameObject colliderHard, GameObject meshEasy, GameObject meshMiddle, GameObject meshHard)
        {
            _fieldSize = new Vector2(
                _boardSize.x - _borderThickness * 2,
                _boardSize.y / 2 - _borderThickness);
            
            _crossbar = new Crossbar(difficult, colliderEasy, colliderMiddle, colliderHard, meshEasy, meshMiddle, meshHard);
            _puck = new Puck();
            _rubberRope = new RubberRope(_fieldSize.x);
        }
        
        /// <summary>
        ///     Удерживать заданную точку в пределах поля
        /// </summary>
        /// <param name="side">Сторона поля</param>
        /// <param name="point">Точка</param>
        public void StayAtField(BoardSide side, ref Vector3 point)
        {
            var xSign = Mathf.Sign(point.x);
            var ySign = side == BoardSide.Upper ? 1 : -1;

            var weight = _fieldSize.x / 2 - _puck.Radius;
            var height = _fieldSize.y - _puck.Radius - _rubberRope.Thickness;
        
            weight = Mathf.Abs(point.y) > _rubberRope.PositionY ? weight - _rubberRope.Thickness : weight;

            if (side == BoardSide.Upper)
            {
                point.y = point.y > 0
                    ? point.y > height ? height : point.y
                    : 0;
            }
            else
            {
                point.y = point.y > 0
                    ? 0
                    : point.y > -height ? point.y : -height;
            }

            point.y = Mathf.Abs(point.y) < _crossbar.Height 
                ? Mathf.Abs(point.x) < _crossbar.Width / 2 ? point.y : (_puck.Radius + _crossbar.Height) * ySign
                : point.y;
        
            weight = Mathf.Abs(point.y) < _crossbar.Height 
                ? Mathf.Abs(point.x) < _crossbar.Width / 2 - _puck.Radius ? weight : _crossbar.Width / 2 - _puck.Radius
                : weight;
        
            point.x = Mathf.Abs(point.x) > weight ? weight * xSign : point.x;
        }
        
        public Vector3 BoardSize => _boardSize;

        public float BorderThickness => _borderThickness;
        
        public Vector2 FieldSize => _fieldSize;

        public Crossbar Crossbar => _crossbar;

        public Puck Puck => _puck;

        public RubberRope RubberRope => _rubberRope;
    }
}