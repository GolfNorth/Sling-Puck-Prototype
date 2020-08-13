namespace SlingPuck.Init
{
    public class Puck
    {
        private readonly float _radius;

        public Puck()
        {
            _radius = 2f;
        }

        public float Radius => _radius;
    }
}