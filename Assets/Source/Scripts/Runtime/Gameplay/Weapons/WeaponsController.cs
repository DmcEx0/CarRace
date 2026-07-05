using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer.Unity;

using CarRace.Gameplay.Car;
using CarRace.Gameplay.Configs;
using CarRace.Gameplay.Enemies;
using CarRace.Gameplay.Targeting;
using CarRace.Gameplay.Weapons.Projectiles;

namespace CarRace.Gameplay.Weapons
{
    public class WeaponsController : IInitializable, IAsyncStartable, IFixedTickable, IDisposable
    {
        private const int ProjectilesCount = 15; //ForTest

        private readonly GameConfig _gameConfig;
        private readonly WeaponsProvider _weaponsProvider;
        private readonly PlayerCarModel _carModel;

        private SphereTargetFinder<EnemyView> _sphereTargetFinder;
        private List<BaseProjectileBehaviour> _projectiles;

        private CancellationTokenSource _cts;
        private CancellationTokenSource _weaponFireCts;

        private bool _canFire;

        public WeaponsController(WeaponsProvider weaponsProvider, GameConfig gameConfig, PlayerCarModel carModel)
        {
            _weaponsProvider = weaponsProvider;
            _gameConfig = gameConfig;
            _carModel = carModel;
        }

        public void Initialize()
        {
            _projectiles = new List<BaseProjectileBehaviour>();
            _cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            DisposeWeaponFireCts();

            _canFire = false;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            await UniTask.WaitUntil(() => _weaponsProvider.IsInitialized, cancellationToken: cancellation);
            
            foreach (var weaponSlot in _weaponsProvider.WeaponsSlots)
            {
                weaponSlot.WeaponContext.Subscribe(OnWeaponContextChanged).AddTo(_cts.Token);
            }
            
            _sphereTargetFinder =
                new SphereTargetFinder<EnemyView>(_gameConfig.EnemyLayerMask, _gameConfig.MaxTargetsCountForPlayer);
            
            _canFire = true;
        }

        public void FixedTick()
        {
            if(_canFire == false)
            {
                return;
            }
            
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                _projectiles[i].OnMove(Time.fixedDeltaTime);
            }
        }

        private async UniTask WeaponFireAsync(WeaponContext weaponContext, CancellationToken token)
        {
            while (_canFire)
            {
                var hasTarget =
                    _sphereTargetFinder.TryGetNearest(_carModel.Context.View.transform.position, weaponContext.BaseRange, out var target);

                if (hasTarget)
                {
                    var projectileBehaviour = weaponContext.ProjectilesFactory.Get(target.ViewTransform.position,
                        weaponContext.View.FirePoints[0].position);

                    projectileBehaviour.DetectedEnemy += OnProjectileEnemyDetected;

                    _projectiles.Add(projectileBehaviour);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(weaponContext.BaseFireRate), cancellationToken: token);
            }
        }

        private void OnProjectileEnemyDetected(BaseProjectileBehaviour projectileBehaviour, EnemyView enemyView)
        {
            _projectiles.Remove(projectileBehaviour);
            projectileBehaviour.DetectedEnemy -= OnProjectileEnemyDetected;
        }
        
        private void RemoveProjectiles()
        {
            foreach (var projectileBehaviour in _projectiles)
            {
                projectileBehaviour.DetectedEnemy -= OnProjectileEnemyDetected;
            }
            
            _projectiles.Clear();
        }

        private void OnWeaponContextChanged(WeaponContext weaponContext)
        {
            if (weaponContext == null)
            {
                return;
            }
            
            DisposeWeaponFireCts();
            
            _weaponFireCts = new CancellationTokenSource();
            
            _canFire = false;
            
            RemoveProjectiles();
            
            ConfigureWeaponAsync(weaponContext).Forget();
        }

        private async UniTask ConfigureWeaponAsync(WeaponContext weaponContext)
        {
            await weaponContext.ProjectilesFactory.PrepareAsync(weaponContext, ProjectilesCount, _cts.Token);
         
            _canFire = true;
            
            WeaponFireAsync(weaponContext, _weaponFireCts.Token).Forget();
        }

        private void DisposeWeaponFireCts()
        {
            _weaponFireCts?.Cancel();
            _weaponFireCts?.Dispose();
        }
    }
}