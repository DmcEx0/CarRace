using Animancer;
using CarRace.Gameplay.Car;
using CarRace.Gameplay.Configs;
using CarRace.Gameplay.Targeting;

namespace CarRace.Gameplay.Enemies
{
    public class EnemyContext
    {
        public EnemyView View { get; private set; }
        
        private readonly EnemiesConfig _config;
        
        public float Speed => _config.Speed;
        public float FollowRadius => _config.FollowRadius;
        public float AttackRadius => _config.AttackRadius;
        
        public SphereTargetFinder<CarView> SphereTargetFinder { get; private set; }
        
        public ClipTransition IdleAnimation => _config.IdleAnimation;
        public ClipTransition FollowAnimation => _config.FollowAnimation;
        
        public EnemyContext(EnemyView view, EnemiesConfig config, SphereTargetFinder<CarView> sphereTargetFinder)
        {
            View = view;
            _config = config;
            
            SphereTargetFinder = sphereTargetFinder;
        }
    }
}