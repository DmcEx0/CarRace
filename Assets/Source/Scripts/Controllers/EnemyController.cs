using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace CarRace
{
    public class EnemyController : IInitializable, IStartable
    {
        private readonly EnemiesConfig _enemiesConfig;
        private readonly EnemyView _enemyViewPrefab;
        private readonly Transform _spawnPointContainer;
        
        private List<EnemyView> _spawnedEnemies;

        public EnemyController(EnemiesConfig enemiesConfig, EnemyView enemyViewPrefab, Transform spawnPointContainer)
        {
            _enemiesConfig = enemiesConfig;
            _enemyViewPrefab = enemyViewPrefab;
            _spawnPointContainer = spawnPointContainer;
        }

        public void Initialize()
        {
            _spawnedEnemies = new List<EnemyView>();
        }

        public void Start()
        {
            for (int i = 0; i < _enemiesConfig.Count; i++)
            {
                var instance = GameObject.Instantiate(_enemyViewPrefab, _spawnPointContainer.position, Quaternion.identity);
                
                _spawnedEnemies.Add(instance); 
            }
        }
    }
}
