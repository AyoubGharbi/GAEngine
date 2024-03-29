﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Inputs
{
    class InputData
    {

        #region keyboard
        private bool _escape = false;
        public bool Escape { get => _escape; set => _escape = value; }

        private bool _space = false;
        public bool Space { get => _space; set => _space = value; }
        #endregion

        #region mouse
        private float _mouseDeltaX;
        public float MouseDeltaX { get => _mouseDeltaX; set => _mouseDeltaX = value; }

        #endregion
    }
}
