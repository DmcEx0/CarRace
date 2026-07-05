namespace CarRace.Gameplay.Car
{
    public class PlayerCarContext
    {
        public readonly CarView View;
        
        public PlayerCarContext(CarView view)
        {
            View = view;
        }
    }
}