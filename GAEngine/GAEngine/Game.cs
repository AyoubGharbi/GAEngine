namespace GAEngine
{
    public class Game
    {
        private GAWindow _gaWindow;

        public Game()
        {
            _gaWindow = new GAWindow();
        }

        public void StartGame()
        {
            _gaWindow.Run();
        }
    }
}
