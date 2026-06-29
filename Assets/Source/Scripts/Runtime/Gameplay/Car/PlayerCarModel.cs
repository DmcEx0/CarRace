namespace CarRace.Gameplay.Car
{
    public class PlayerCarModel
    {
        public CarView View { get; private set; }
        
        public void SetView(CarView view)
        {
            View = view;
        }
    }
}