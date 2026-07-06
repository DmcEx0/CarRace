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

        private Dictionary<WeaponSlot, CancellationTokenSource> _fireCtsBySlot;

        private CancellationTokenSource _cts;

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

            _fireCtsBySlot = new Dictionary<WeaponSlot, CancellationTokenSource>();
            _cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _canFire = false;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            await UniTask.WaitUntil(() => _weaponsProvider.IsInitialized, cancellationToken: cancellation);

            foreach (var weaponSlot in _weaponsProvider.WeaponsSlots)
            {
                weaponSlot.WeaponContext
                    .SubscribeAwait(context => OnWeaponContextChanged(weaponSlot, context)).AddTo(_cts.Token);
            }

            _sphereTargetFinder =
                new SphereTargetFinder<EnemyView>(_gameConfig.EnemyLayerMask, _gameConfig.MaxTargetsCountForPlayer);

            _canFire = true;
        }

        public void FixedTick()
        {
            if (_canFire == false)
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
            while (token.IsCancellationRequested == false)
            {
                if (_sphereTargetFinder.TryGetNearest(_carModel.Context.View.transform.position,
                        weaponContext.BaseRange, out var target))
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

        private void RemoveProjectiles() //TODO: чистит все прожектайлы, а не конкретного слота
        {
            foreach (var projectileBehaviour in _projectiles)
            {
                projectileBehaviour.DetectedEnemy -= OnProjectileEnemyDetected;
            }

            _projectiles.Clear();
        }

        private async UniTask OnWeaponContextChanged(WeaponSlot slot, WeaponContext weaponContext)
        {
            StopFireForSlot(slot);

            if (weaponContext == null)
            {
                return;
            }

            RemoveProjectiles();

            var cts = new CancellationTokenSource();
            _fireCtsBySlot[slot] = cts;

            await weaponContext.ProjectilesFactory.PrepareAsync(weaponContext, ProjectilesCount, _cts.Token);

            WeaponFireAsync(weaponContext, cts.Token).Forget();
        }
        
        private void StopFireForSlot(WeaponSlot slot)
        {
            if (_fireCtsBySlot.Remove(slot, out var oldCts))
            {
                oldCts.Cancel();
                oldCts.Dispose();
            }
        }
    }
}