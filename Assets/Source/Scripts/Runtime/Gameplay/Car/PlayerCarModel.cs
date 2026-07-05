namespace CarRace.Gameplay.Car
{
    public class PlayerCarModel
    {
        public PlayerCarContext Context { get; private set; }
        
        public void SetContext(PlayerCarContext context)
        {
            Context = context;
        }
    }
}