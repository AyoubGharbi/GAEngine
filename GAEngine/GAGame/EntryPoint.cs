using GAEngine;
using OpenTK;
using System;

namespace GAGame
{
    class EntryPoint
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
