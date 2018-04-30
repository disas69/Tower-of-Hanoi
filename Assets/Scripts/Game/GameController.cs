using System.Collections;
using System.Collections.Generic;
using Framework.Tools.Gameplay;
using Framework.UI;
using Framework.Variables;
using Game.Gameplay;
using Game.Gameplay.RingStructure.Configuration;
using Game.UI;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        private List<Ring> _activeRings;
        private StateMachine<GameState> _gameStateMachine;
        private List<Ring> _ringsPrefabs;

        [SerializeField] private List<Tower> _towers;
        [SerializeField] private int _ringsCount;
        [SerializeField] private Transform _ringsRoot;
        [SerializeField] private RingsStorage _ringsStorage;
        [SerializeField] private IntVariable _movesCoutVariable;
        [SerializeField] private NavigationProvider _navigation;

        private void Start()
        {
            Input.multiTouchEnabled = false;

            _activeRings = new List<Ring>();
            _gameStateMachine = new StateMachine<GameState>();
            _gameStateMachine.AddTransition(GameState.Idle, GameState.Play, StartGame);
            _gameStateMachine.AddTransition(GameState.Play, GameState.Stop, StopGame);
            _gameStateMachine.AddTransition(GameState.Stop, GameState.Play, StartGame);
            _gameStateMachine.SetState(GameState.Idle);

            _ringsPrefabs = _ringsStorage.GetRingsPrefabs();

            if (_navigation != null)
            {
                _navigation.OpenScreen<StartPage>();
            }
            else
            {
                Debug.LogError("NavigationProvider is not assigned");
            }
        }

        public void Play()
        {
            _gameStateMachine.SetState(GameState.Play);
        }

        public void Stop()
        {
            _gameStateMachine.SetState(GameState.Stop);
        }

        private void StartGame()
        {
            Reset();

            if (_navigation != null)
            {
                _navigation.OpenScreen<PlayPage>();
            }

            if (_towers.Count > 0)
            {
                var startupTower = _towers[Random.Range(0, _towers.Count)];

                for (int i = 0; i < _towers.Count; i++)
                {
                    var tower = _towers[i];
                    tower.Setup(tower != startupTower, _ringsCount);
                }

                StartCoroutine(SpawnRings(startupTower));
            }
            else
            {
                Debug.LogError("Towers list is empty");
            }
        }

        private void StopGame()
        {
            if (_navigation != null)
            {
                _navigation.OpenScreen<ReplayPage>();
            }
        }

        private IEnumerator SpawnRings(Tower tower)
        {
            var waiter = new WaitForSeconds(0.15f);

            for (int i = 0; i < _ringsCount; i++)
            {
                var ring = Instantiate(_ringsPrefabs[i], _ringsRoot);
                ring.AttachTo(tower, false);

                var spawnPosition = tower.Spawn.position;
                spawnPosition.z = 0f;
                ring.transform.position = spawnPosition;

                _activeRings.Add(ring);
                yield return waiter;
            }
        }

        private void Reset()
        {
            for (int i = 0; i < _activeRings.Count; i++)
            {
                Destroy(_activeRings[i].gameObject);
            }

            _activeRings.Clear();
            _movesCoutVariable.Value = 0;
        }
    }
}