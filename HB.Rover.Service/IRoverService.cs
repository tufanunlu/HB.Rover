using System;
using System.Collections.Generic;
using System.Text;

namespace HB.Rover.Service
{
    public interface IRoverService
    {
        List<string> RunCommands(string commands);
    }
}
