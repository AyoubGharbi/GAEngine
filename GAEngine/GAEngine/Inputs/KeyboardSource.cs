using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Inputs
{
    class KeyboardSource : ISource
    {
        public void ClearData(InputData data)
        {
            data.Escape = false;
        }

        public void Update(InputData data)
        {
            var keyboardState = Keyboard.GetState();
            data.Escape |= keyboardState.IsKeyDown(Key.Escape);
        }
    }
}
