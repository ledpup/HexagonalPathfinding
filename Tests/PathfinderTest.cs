using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Model;

namespace Tests
{
    /// <summary>
    ///This is a test class for PathfinderTest and is intended
    ///to contain all PathfinderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PathfinderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void SimplePathTest()
        {
            var start = new Tile(0, 0);
            var tile2 = new Tile(1, 0);
            var tile3 = new Tile(1, 1);
            var destination = new Tile(1, 2);

            start.AllNeighbours = new List<Tile> { tile2 };
            tile2.AllNeighbours = new List<Tile> { tile3 };
            tile3.AllNeighbours = new List<Tile> { destination };

            Func<Tile, Tile, double> distance = (node1, node2) => 1;

            //node1.Neighbours.Cast<EdgeToNeighbor>().Single(etn => etn.Neighbour.Key == node2.Key).Cost
            Func<Tile, double> manhattanEstimation = n => Math.Abs(n.X - destination.X) + Math.Abs(n.Y - destination.Y);

            var path = PathFind.PathFind.FindPath(start, destination, distance, manhattanEstimation);

            Assert.IsTrue(path.TotalCost == 3);
        }

        [TestMethod()]
        public void ComplexPathTest()
        {
            var game = new Game(8, 8);

            var start = game.GameBoard[0, 0];
            var end = game.GameBoard[7, 7];

            Func<Tile, Tile, double> distance = (node1, node2) => 1;
            Func<Tile, double> manhattanEstimation = n => Math.Abs(n.X - end.X) + Math.Abs(n.Y - end.Y);

            var path = PathFind.PathFind.FindPath(start, end, distance, manhattanEstimation);

            Assert.IsTrue(path.TotalCost == 11);
        }


    }
}
