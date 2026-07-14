using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuvolics.Utilities;

internal struct CataclysmState
{
    public CataclysmState()
    {
        state = 3;
    }

    private int state;

    public int ZephironCount
    {
        readonly get
        {
            return state & 7;
        }
        set
        {
            state &= ~7;
            state |= (value & 7);
        }
    }
}
