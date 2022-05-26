using System.Collections;
using UnityEngine;

namespace Assets._Game.Scripts.Interfaces
{
    public interface IInteractableWithPlayer
    {
        bool IsInteracted { get; set; }
        bool IsEnabled { get; set; }

        void OnInteracted();
        void OnExit();
    }
}