using Animancer;

namespace CarRace
{
    public class EnemyContext
    {
        public EnemyView View { get; private set; }
        public EnemiesConfig Config { get; private set; }
        
        public EnemyContext(EnemyView view, EnemiesConfig config)
        {
            View = view;
            Config = config;
        }
    }
}