namespace GAEngine
{
    class Game
    {
        static void Main(string[] args)
        {
            using (GAWindow gaWindow = new GAWindow())
            {
                gaWindow.Run(60.0);
            }
        }
    }
}
