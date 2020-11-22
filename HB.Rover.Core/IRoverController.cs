using HB.Rover.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace HB.Rover.Core
{
    public interface IRoverController
    {
        void SetStartPosition(DirectionState direction, int x, int y);
        void SetPlateauUpperRight(int x, int y);
        void Move();
        void Left();
        void Right();
        string GetCurrentState();

    }
}
