using UnityEngine;

namespace SlingPuck.Init
{
    public class RubberRope
    {
        private readonly float _height;
        private readonly float _width;
        private readonly float _thickness;
        private readonly float _positionY;
        private readonly float _maxTension;
        private readonly float _maxForce;

        public RubberRope(float width)
        {
            _height = 5f;
            _width = width;
            _thickness = 0.3f;
            _positionY = 23f;
            _maxForce = 600f;
            
            var startPoint = new Vector2(0,0);
            var endPoint = new Vector2(_width, 0);
            var maxTensionPoint = new Vector2(0, _height);
            
            _maxTension = Vector3.Distance(startPoint, maxTensionPoint)
                          + Vector3.Distance(endPoint, maxTensionPoint) 
                          - width;
        }

        public float Height => _height;

        public float Width => _width;

        public float Thickness => _thickness;

        public float PositionY => _positionY;

        public float MaxTension => _maxTension;

        public float MaxForce => _maxForce;
    }
}