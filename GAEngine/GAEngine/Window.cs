using OpenTK;
using OpenTK.Graphics;

namespace GAEngine
{
    public class Window
    {
        private readonly GameWindow _gameWindow;

        public Window(string title, int width, int height)
        {
            _gameWindow = new GameWindow(width, height, GraphicsMode.Default, title);
        }

        public void Start()
        {
            _gameWindow.Run(1.0 / 60.0);
        }
    }
}
