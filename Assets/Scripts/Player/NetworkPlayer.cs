using System;
using UnityEngine;

namespace SlingPuck.Player
{
    public class NetworkPlayer : IPlayer
    {
        public event EventHandler<GameObject> PuckGrabbed;
        public event EventHandler<GameObject> PuckReleased;
        
        public GameObject GetPuck()
        {
            throw new NotImplementedException();
        }

        public Vector3 GetPosition()
        {
            throw new NotImplementedException();
        }

        public void SetPosition()
        {
            throw new NotImplementedException();
        }

        public bool HasPuck()
        {
            throw new NotImplementedException();
        }
    }
}