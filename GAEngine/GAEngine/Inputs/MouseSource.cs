using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Inputs
{
    class MouseSource : ISource
    {
        public void ClearData(InputData data)
        {
            data.MouseDeltaX = 0.0f;
        }

        public void Update(InputData data)
        {
            var mouseState = Mouse.GetCursorState();

            if (mouseState.IsButtonDown(MouseButton.Left))
                data.MouseDeltaX +=  0.005f;
        }
    }
}
