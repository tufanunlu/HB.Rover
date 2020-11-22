using HB.Rover.Core.Enum;
using Stateless;
using System;

namespace HB.Rover.Core
{
    public class RoverController : IRoverController
    {
        public enum Trigger
        {
            Right,
            Left,
            Move
        }

        public RoverController()
        {
            _machine = new StateMachine<DirectionState, Trigger>(() => _state, s => _state = s);

            _machine.Configure(DirectionState.N)
                .InternalTransition(Trigger.Move, t => MoveNorth())
                .InternalTransition(Trigger.Left, t => ChangeDirectionToWest())
                .InternalTransition(Trigger.Right, t => ChangeDirectionToEast());

            _machine.Configure(DirectionState.S)
                .InternalTransition(Trigger.Move, t => MoveSouth())
                .InternalTransition(Trigger.Left, t => ChangeDirectionToEast())
                .InternalTransition(Trigger.Right, t => ChangeDirectionToWest());

            _machine.Configure(DirectionState.E)
                .InternalTransition(Trigger.Move, t => MoveEast())
                .InternalTransition(Trigger.Left, t => ChangeDirectionToNorth())
                .InternalTransition(Trigger.Right, t => ChangeDirectionToSouth());

            _machine.Configure(DirectionState.W)
                .InternalTransition(Trigger.Move, t => MoveWest())
                .InternalTransition(Trigger.Left, t => ChangeDirectionToSouth())
                .InternalTransition(Trigger.Right, t => ChangeDirectionToNorth());
        }

        StateMachine<DirectionState, Trigger> _machine;
        DirectionState _state;
        int _x = 0, _y = 0;
        int _PlateauUpperRightX = 0, _PlateauUpperRightY = 0;

        public void SetStartPosition(DirectionState direction, int x, int y)
        {
            ValidateLocation(x, y);

            _state = direction;
            _x = x;
            _y = y;
        }

        public void SetPlateauUpperRight(int x, int y)
        {
            _PlateauUpperRightX = x;
            _PlateauUpperRightY = y;
        }

        void MoveNorth()
        {
            _y++;
        }
        void MoveSouth()
        {
            _y--;
        }

        void MoveWest()
        {
            _x--;
        }

        void MoveEast()
        {
            _x++;
        }

        void ChangeDirectionToWest()
        {
            _state = DirectionState.W;
        }

        void ChangeDirectionToEast()
        {
            _state = DirectionState.E;
        }

        void ChangeDirectionToNorth()
        {
            _state = DirectionState.N;
        }

        void ChangeDirectionToSouth()
        {
            _state = DirectionState.S;
        }


        public void Move()
        {
            _machine.Fire(Trigger.Move);
        }

        public void Left()
        {
            _machine.Fire(Trigger.Left);
        }

        public void Right()
        {
            _machine.Fire(Trigger.Right);
        }

        public string GetCurrentState()
        {
            ValidateLocation(_x, _y);
            return string.Format("{0} {1} {2}", _x, _y, _state.ToString());
        }

        void ValidateLocation(int x, int y)
        {
            if (x > _PlateauUpperRightX || y > _PlateauUpperRightY || x < 0 || y < 0)
                throw new Exception("Out of plateau");
        }
    }
}
