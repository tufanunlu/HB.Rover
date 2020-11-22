using HB.Rover.Core;
using HB.Rover.Service;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Tests
{
    public class RoverTests
    {
        private ServiceProvider serviceProvider { get; set; }

        [SetUp]
        public void Setup()
        {
            serviceProvider = new ServiceCollection()
                                       .AddTransient<IRoverService, RoverService>()
                                       .AddTransient<IRoverController, RoverController>()
                                       .BuildServiceProvider();
        }

        [Test]
        public void HappyPath()
        {
            var service = serviceProvider.GetService<IRoverService>();
            var result = service.RunCommands(@"5 5
1 2 N
LMLMLMLMM
3 3 E
MMRMMRMRRM");

            Assert.AreEqual(
                @"1 3 N | 5 1 E", string.Join(" | ", result));

        }

        [Test]
        public void HappyPath2()
        {
            var service = serviceProvider.GetService<IRoverService>();
            var result = service.RunCommands(@"6 6
1 2 N
LMLMLMLMM
3 3 E
MMRMMRMRRM
3 1 W
MMRMMMMM");

            Assert.AreEqual(
                @"1 3 N | 5 1 E | 1 6 N", string.Join(" | ", result));

        }

        [Test]
        public void InvalidInput()
        {
            var service = serviceProvider.GetService<IRoverService>();

            Assert.That(() => service.RunCommands(@"5 5
1 2 N"),
                Throws.TypeOf<ArgumentException>().With.Message.Contains("Invalid input"));
        }


        [Test]
        public void StartPositionOutOfPlateau()
        {
            var service = serviceProvider.GetService<IRoverService>();
            Assert.That(() => service.RunCommands(@"5 5
6 8 N
LMLMLMLMM
3 3 E
MMRMMRMRRM"),
Throws.TypeOf<Exception>().With.Message.Contains("Out of plateau"));

        }


        [Test]
        public void ResultOutOfPlateau()
        {
            var service = serviceProvider.GetService<IRoverService>();
            Assert.That(() => service.RunCommands(@"5 5
4 4 N
MMM
3 3 E
MMRMMRMRRM"),
Throws.TypeOf<Exception>().With.Message.Contains("Out of plateau"));

        }

    }
}