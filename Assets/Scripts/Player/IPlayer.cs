using System;
using UnityEngine;

namespace SlingPuck.Player
{
    public interface IPlayer
    {
        event EventHandler<GameObject> PuckGrabbed;
        event EventHandler<GameObject> PuckReleased;
        
        GameObject GetPuck();
        
        Vector3 GetPosition();

        void SetPosition();

        bool HasPuck();
    }
}