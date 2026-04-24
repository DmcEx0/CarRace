using System;
using System.Collections.Generic;
using System.Threading;
using CarRace.Helpers;
using CarRace.Views;
using CarRace.Weapon;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer.Unity;

namespace CarRace.Controllers
{
    public class WeaponsController : IInitializable, IStartable, IFixedTickable, IDisposable
    {
        private const int ProjectilesCount = 15; //ForTest

        private readonly GameConfig _gameConfig;
        private readonly WeaponsProvider _weaponsProvider;
        private readonly ProjectilesFactory _projectilesFactory;
        private readonly CarView _view;

        private TargetSystem<EnemyView> _targetSystem;
        private List<BaseProjectileBehaviour> _projectiles;

        private CancellationTokenSource _cts;

        private bool _canFire;

        public WeaponsController(WeaponsProvider weaponsProvider, GameConfig gameConfig,
            ProjectilesFactory projectilesFactory, CarView view)
        {
            _weaponsProvider = weaponsProvider;
            _gameConfig = gameConfig;
            _projectilesFactory = projectilesFactory;
            _view = view;
        }

        public void Initialize()
        {
            _targetSystem =
                new TargetSystem<EnemyView>(_gameConfig.EnemyLayerMask, _gameConfig.MaxTargetsCountForPlayer);

            _projectiles = new List<BaseProjectileBehaviour>();
            _cts = new CancellationTokenSource();
            _canFire = true;
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _canFire = false;
        }

        public void Start()
        {
            foreach (var weaponSlot in _weaponsProvider.WeaponsSlots)
            {
                weaponSlot.WeaponContext.Subscribe(OnWeaponContextChanged).AddTo(_cts.Token);
            }
        }

        public void FixedTick()
        {
            foreach (var projectile in _projectiles)
            {
                projectile.OnMove(Time.fixedDeltaTime);
            }
        }

        private async UniTask WeaponFireAsync(WeaponContext weaponContext)
        {
            while (_canFire)
            {
                var hasTarget =
                    _targetSystem.TryGetNearest(out var target, _view.transform.position, weaponContext.BaseRange);

                if (hasTarget)
                {
                    var projectileBehaviour = _projectilesFactory.Get(target.transform.position,
                        weaponContext.View.FirePoints[0].position);

                    projectileBehaviour.DetectedEnemy += OnProjectileEnemyDetected;

                    _projectiles.Add(projectileBehaviour);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(weaponContext.BaseFireRate), cancellationToken: _cts.Token);
            }
        }

        private void OnProjectileEnemyDetected(BaseProjectileBehaviour projectileBehaviour, EnemyView enemyView)
        {
            _projectiles.Remove(projectileBehaviour);
            projectileBehaviour.DetectedEnemy -= OnProjectileEnemyDetected;
        }

        private void OnWeaponContextChanged(WeaponContext weaponContext)
        {
            if (weaponContext == null)
            {
                return;
            }

            ConfigureWeaponAsync(weaponContext).Forget();
        }

        private async UniTask ConfigureWeaponAsync(WeaponContext weaponContext)
        {
            await _projectilesFactory.PrepareAsync(weaponContext, ProjectilesCount, _cts.Token);
            await WeaponFireAsync(weaponContext);
        }
    }
}