using UnityEngine;
using SlingPuck.Enum;

namespace SlingPuck.Init
{
    public class Crossbar
    {
        private float _width;
        private readonly float _height = 1f;
        private readonly float _widthEasy = 5.6f;
        private readonly float _widthMiddle = 5.0f;
        private readonly float _widthHard = 4.4f;
        private readonly GameObject _colliderEasy;
        private readonly GameObject _colliderMiddle;
        private readonly GameObject _colliderHard;
        private readonly GameObject _meshEasy;
        private readonly GameObject _meshMiddle;
        private readonly GameObject _meshHard;

        public Crossbar(Difficult difficult, GameObject colliderEasy, GameObject colliderMiddle, GameObject colliderHard, GameObject meshEasy, GameObject meshMiddle, GameObject meshHard)
        {
            _colliderEasy = colliderEasy;
            _colliderMiddle = colliderMiddle;
            _colliderHard = colliderHard;

            _meshEasy = meshEasy;
            _meshMiddle = meshMiddle;
            _meshHard = meshHard;
            
            SwitchDifficult(difficult);
        }

        public void SwitchDifficult(Difficult difficult)
        {
            switch (difficult)
            {
                case Difficult.Middle:
                    _width = _widthMiddle;
                    
                    _colliderEasy.SetActive(false);
                    _colliderMiddle.SetActive(true);
                    _colliderHard.SetActive(false);

                    _meshEasy.SetActive(false);
                    _meshMiddle.SetActive(true);
                    _meshHard.SetActive(false);
                    
                    break;
                case Difficult.Hard:
                    _width = _widthHard;
                    
                    _colliderEasy.SetActive(false);
                    _colliderMiddle.SetActive(false);
                    _colliderHard.SetActive(true);

                    _meshEasy.SetActive(false);
                    _meshMiddle.SetActive(false);
                    _meshHard.SetActive(true);
                    
                    break;
                case Difficult.Easy:
                default:
                    _width = _widthEasy;
                    
                    _colliderEasy.SetActive(true);
                    _colliderMiddle.SetActive(false);
                    _colliderHard.SetActive(false);

                    _meshEasy.SetActive(true);
                    _meshMiddle.SetActive(false);
                    _meshHard.SetActive(false);
                    
                    break;
            }
            
            
        }

        public float Width => _width;

        public float Height => _height;
    }
}