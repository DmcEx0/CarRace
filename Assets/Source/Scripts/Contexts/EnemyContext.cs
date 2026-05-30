using Animancer;
using CarRace.Configs;
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

        public ClipTransition IdleAnimation => _config.IdleAnimation;
        public ClipTransition FollowAnimation => _config.FollowAnimation;
        
        public EnemyContext(EnemyView view, EnemiesConfig config)
        {
            View = view;
            _config = config;
        }
    }
}