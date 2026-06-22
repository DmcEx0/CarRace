using Animancer;
using CarRace.Configs;
using CarRace.Helpers;
using CarRace.Views;

namespace CarRace.Contexts
{
    public class EnemyContext
    {
        public EnemyView View { get; private set; }
        
        private readonly EnemiesConfig _config;
        
        public float Speed => _config.Speed;
        public float FollowRadius => _config.FollowRadius;
        public float AttackRadius => _config.AttackRadius;
        
        public TargetFinder<CarView> TargetFinder { get; private set; }
        
        public ClipTransition IdleAnimation => _config.IdleAnimation;
        public ClipTransition FollowAnimation => _config.FollowAnimation;
        
        public EnemyContext(EnemyView view, EnemiesConfig config, TargetFinder<CarView> targetFinder)
        {
            View = view;
            _config = config;
            
            TargetFinder = targetFinder;
        }
    }
}