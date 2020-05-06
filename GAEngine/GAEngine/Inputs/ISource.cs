using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Inputs
{
    interface ISource
    {
        void ClearData(InputData data);

        void Update(InputData data);
    }
}
