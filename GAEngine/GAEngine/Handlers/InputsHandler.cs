using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Inputs
{
    class InputsHandler
    {
        private InputData _inputData;
        private MouseState _mouseState;
        private KeyboardState _keyboardState;
        private List<ISource> _inputSources = new List<ISource>();

        public InputData Data => _inputData;

        public InputsHandler()
        {
            _inputData = new InputData();

            _inputSources.Add(new MouseSource());
            _inputSources.Add(new KeyboardSource());
        }

        public void Update()
        {
            Clear();

            foreach (var source in _inputSources)
                source.Update(_inputData);
        }

        void Clear()
        {
            foreach (var source in _inputSources)
                source.ClearData(_inputData);
        }
    }
}
