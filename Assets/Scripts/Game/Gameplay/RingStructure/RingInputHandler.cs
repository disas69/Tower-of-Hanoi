using System;
using Framework.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Gameplay.RingStructure
{
    public class RingInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private Camera _mainCamera;

        public event Action<Vector2> PointerDown;
        public event Action<Vector2> Drag;
        public event Action PointerUp;

        public bool IsDragging { get; private set; }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsDragging = true;
            PointerDown.SafeInvoke(_mainCamera.ScreenToWorldPoint(eventData.position));
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag.SafeInvoke(_mainCamera.ScreenToWorldPoint(eventData.position));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsDragging = false;
            PointerUp.SafeInvoke();
        }
    }
}