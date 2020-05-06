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
        private KeyboardState _previousSate;

        public void ClearData(InputData data)
        {
            data.Escape = false;
            data.Space = false;
        }

        public void Update(InputData data)
        {
            var keyboardState = Keyboard.GetState();
            data.Escape |= keyboardState.IsKeyDown(Key.Escape);
            data.Space |= keyboardState.IsKeyDown(Key.Space) && _previousSate.IsKeyUp(Key.Space);

            _previousSate = keyboardState;
        }
    }
}
