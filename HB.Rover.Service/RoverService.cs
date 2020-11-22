using HB.Rover.Core;
using HB.Rover.Core.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HB.Rover.Service
{
    public class RoverService : IRoverService
    {

        private readonly IRoverController _controller;
        public RoverService(IRoverController controller)
        {
            _controller = controller;
        }

        public List<string> RunCommands(string commands)
        {
            List<string> results = new List<string>();

            List<string> _cmdArray = new List<string>();

            using (StringReader sr = new StringReader(commands))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    _cmdArray.Add(line);
                }
            }

            if (_cmdArray.Count < 3)
                throw new ArgumentException("Invalid input");

            for (int i = 0; i < _cmdArray.Count; i++)
            {
                if (i == 0) //first row
                {
                    string[] plateauUpperRight = _cmdArray[0].Split(' ');
                    if (plateauUpperRight == null || plateauUpperRight.Length != 2)
                        throw new ArgumentException("Invalid input");

                    _controller.SetPlateauUpperRight(Convert.ToInt32(plateauUpperRight[0]), Convert.ToInt32(plateauUpperRight[1]));

                    continue;
                }

                if (i % 2 == 1) //rover's position
                {
                    var _position = _cmdArray[i].Split(' ');
                    _controller.SetStartPosition((DirectionState)Enum.Parse(typeof(DirectionState), _position[2]),
                        Convert.ToInt32(_position[0]), Convert.ToInt32(_position[1]));
                }
                else if (i % 2 == 0) //series of instructions telling the rover how to explore the plateau
                {
                    var _commandArray = _cmdArray[i].ToCharArray();
                    foreach (var _command in _commandArray)
                    {
                        switch (_command)
                        {
                            case 'R':
                                _controller.Right();
                                break;
                            case 'L':
                                _controller.Left();
                                break;
                            case 'M':
                                _controller.Move();
                                break;
                            default:
                                break;
                        }
                    }

                    results.Add(_controller.GetCurrentState());
                }
            }

            return results;
        }


    }
}
