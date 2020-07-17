using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using RustCrossbreed.BusinessLogic;

namespace RustCrossbreed.Tests
{
    [TestClass]
    public class RustCrossbreederTests
    {
        [DataTestMethod]
        [DataRow(new EGene[] { EGene.W, EGene.G, EGene.H, EGene.Y }, new EGene[] { EGene.W })]
        [DataRow(new EGene[] { EGene.W, EGene.G, EGene.G, EGene.Y }, new EGene[] { EGene.G })]
        [DataRow(new EGene[] { EGene.Y, EGene.G, EGene.G, EGene.Y }, new EGene[] { EGene.G, EGene.Y })]
        [DataRow(new EGene[] { EGene.W, EGene.X, EGene.G, EGene.G }, new EGene[] { EGene.G })]
        [DataRow(new EGene[] { EGene.W, EGene.X, EGene.G, EGene.Y }, new EGene[] { EGene.W, EGene.X })]
        [DataRow(new EGene[] { EGene.G, EGene.G, EGene.G, EGene.G }, new EGene[] { EGene.G })]
        [DataRow(new EGene[] { EGene.H, EGene.G, EGene.Y }, new EGene[] { EGene.H, EGene.G, EGene.Y })]
        public void CrossbreedGenes(EGene[] input, EGene[] expected)
        {
            //Arrange
            var crossbreeder = new RustCrossbreeder();

            //Act
            List<EGene> actual = crossbreeder.CrossbreedGenes(input);

            //Assert
            CollectionAssert.AreEquivalent(expected.ToList(), actual);
        }

        [TestMethod]
        public void Crossbreed()
        {
            //Arrange 
            var crossbreeder = new RustCrossbreeder();
            var expected = new List<EGene>() { EGene.G, EGene.G, EGene.Y, EGene.Y, EGene.Y, EGene.Y };
            var input = new List<List<EGene>>()
            {
                new List<EGene>() { EGene.W, EGene.X, EGene.G, EGene.Y, EGene.Y, EGene.G },
                new List<EGene>() { EGene.H, EGene.G, EGene.Y, EGene.Y, EGene.H, EGene.W },
                new List<EGene>() { EGene.G, EGene.W, EGene.H, EGene.X, EGene.G, EGene.Y },
                new List<EGene>() { EGene.G, EGene.G, EGene.Y, EGene.G, EGene.Y, EGene.Y }
            };

            //Act
            List<List<EGene>> actual = crossbreeder.CrossbreedSimple(input);

            //Assert
            CollectionAssert.AreEqual(expected, actual[0]);
        }

        ///alright, so I don't actually know a good way to test this, so I just used the debugger to make sure it working as intended
        ///the reason why i don't know a good way is because even if you were to loop through and assert equivalent for all permutations,
        ///you would still need to manually type out the results.
        ///to test, just make the method public and then run test and debug it with a break to check to see if it's working
        //[TestMethod]
        //public void PurmutatePossibleGenes()
        //{
        //    //Arrange 
        //    var crossbreeder = new RustCrossbreeder();
        //    var expected = new List<EGene>() { EGene.G, EGene.G, EGene.Y, EGene.Y, EGene.Y, EGene.Y };
        //    var input = new List<List<EGene>>()
        //        {
        //            new List<EGene>() { EGene.Y, EGene.G, EGene.H},
        //            new List<EGene>() { EGene.Y},
        //            new List<EGene>() { EGene.G, EGene.Y },
        //            new List<EGene>() { EGene.G },
        //            new List<EGene>() { EGene.X, EGene.W },
        //            new List<EGene>() { EGene.G}
        //        };

        //    //Act
        //    List<List<EGene>> actual = crossbreeder.PurmutatePossibleGenes(input);

        //    //Assert
        //    Assert.IsTrue(actual.Count == 12);
        //}
    }
}
