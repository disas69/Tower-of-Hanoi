using System.Collections.Generic;
using Framework.Variables;
using Game.Gameplay.TowerStructure.Configuration;
using UnityEngine;
using Event = Framework.Events.Event;

namespace Game.Gameplay
{
    public class Tower : MonoBehaviour
    {
        private readonly Stack<Ring> _ringsStack = new Stack<Ring>();

        private bool _completionCheckEnabled;
        private int _ringsCount;

        [SerializeField] private Transform _anchor;
        [SerializeField] private Transform _spawn;
        [SerializeField] private Transform _bottom;
        [SerializeField] private TowerSettings _settings;
        [SerializeField] private IntVariable _movesCountVariable;
        [SerializeField] private Event _onCompletedEvent;

        public Transform Anchor
        {
            get { return _anchor; }
        }

        public Transform Spawn
        {
            get { return _spawn; }
        }

        public Transform Bottom
        {
            get { return _bottom; }
        }

        public void Setup(bool completionCheckEnabled, int ringsCount)
        {
            _completionCheckEnabled = completionCheckEnabled;
            _ringsCount = ringsCount;
            _ringsStack.Clear();
        }

        public void Add(Ring ring, bool isCountable)
        {
            _ringsStack.Push(ring);

            if (isCountable)
            {
                _movesCountVariable.Value++;
            }

            if (_completionCheckEnabled && _ringsStack.Count == _ringsCount)
            {
                _onCompletedEvent.Fire();
            }
        }

        public Vector2 GetTopPosition()
        {
            var ringsCount = _ringsStack.Count > 0 ? _ringsStack.Count - 1 : 0;
            var yPosition = _bottom.transform.position.y + _settings.RingAnchoringStep * ringsCount;
            return new Vector2(_bottom.transform.position.x, yPosition);
        }

        public void Remove()
        {
            if (_ringsStack.Count > 0)
            {
                _ringsStack.Pop();
            }
        }

        public int GetTopRingSizeIndex()
        {
            if (_ringsStack.Count > 0)
            {
                return _ringsStack.Peek().SizeIndex;
            }

            return -1;
        }
    }
}