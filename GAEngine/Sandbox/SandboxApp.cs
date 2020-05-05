using System;
using GAEngine;

namespace Sandbox
{
    class SandboxApp
    {
        static void Main(string[] args)
        {
            Sandbox sandbox = new Sandbox();
            sandbox.StartGame();
        }
    }

    public class Sandbox : Game
    {
    }
}
