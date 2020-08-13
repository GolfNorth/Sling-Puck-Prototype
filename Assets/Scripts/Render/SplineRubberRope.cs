using SplineMesh;
using UnityEngine;
using SlingPuck.Behaviours;

namespace SlingPuck.Render
{
    public class SplineRubberRope : MonoBehaviour
    {
        [SerializeField]
        private RubberRopeBehaviour rubberRope;

        private Spline _spline;

        private void Awake()
        {
            _spline = GetComponent<Spline>();
        }

        private void Update()
        {
            _spline.nodes[2].Position = rubberRope.TangentA;
            _spline.nodes[3].Position = rubberRope.LaunchPosition;
            _spline.nodes[4].Position = rubberRope.TangentB;
        }
    }
}
