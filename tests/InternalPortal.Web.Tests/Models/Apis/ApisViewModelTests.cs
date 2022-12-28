using InternalPortal.Web.Models.Apis;

namespace InternalPortal.Web.Tests.Models.Apis
{
    [TestClass]
    public class ApisViewModelTests
    {
        [TestMethod]
        [DataRow(0, 0, 0)]
        [DataRow(null, 0, 0)]
        [DataRow(-1, -1, -1)]
        public void ApisViewModel_WorksWith_NoData(int? total, int skipped, int taken)
        {
            // act
            var model = new ApisViewModel(total, skipped, taken);

            // assert
            Assert.IsNotNull(model);
        }

        [TestMethod]
        [DataRow(0, 0, 0, 0)]
        [DataRow(10, 0, 1, 10)]
        [DataRow(9, 0, 10, 1)]
        [DataRow(100, 0, 9, 12)]
        public void ApisViewModel_TotalPages_RoundsUp(int total, int skipped, int taken, int expected)
        {
            // act
            var model = new ApisViewModel(total, skipped, taken);

            // assert
            Assert.AreEqual(expected, model.TotalPages);
        }

        [TestMethod]
        [DataRow(0, 1, 0, true)]
        [DataRow(0, 100, 0, true)]
        [DataRow(0, 0, 0, false)]
        [DataRow(0, -1, 0, false)]
        public void ApisViewModel_ShowsPreviousPageLink(int total, int skipped, int taken, bool expected)
        {
            // act
            var model = new ApisViewModel(total, skipped, taken);

            // assert
            Assert.AreEqual(expected, model.ShowPreviousLink);
        }

        [TestMethod]
        [DataRow(0, 11, 5, 3)]
        [DataRow(0, 10, 5, 2)]
        [DataRow(0, 9, 5, 2)]
        [DataRow(0, 6, 5, 2)]
        public void ApisViewModel_PreviousPage_RoundsUp(int total, int skipped, int taken, int expected)
        {
            // act
            var model = new ApisViewModel(total, skipped, taken);

            // assert
            Assert.AreEqual(expected, model.PreviousPage);
        }

        [TestMethod]
        [DataRow(0, 10, 5, "/apis?skip=5&take=5")]
        [DataRow(0, 15, 10, "/apis?skip=5&take=10")]
        [DataRow(0, 0, 5, "/apis?skip=-5&take=5")]
        public void ApisViewModel_PreviousPageLink_LinksToPreviousSet(int total, int skipped, int taken, string expected)
        {
            // act
            var model = new ApisViewModel(total, skipped, taken);

            // assert
            Assert.AreEqual(expected, model.PreviousPageLink);
        }

        [TestMethod]
        [DataRow(1, 1, 1, false)]
        [DataRow(2, 1, 1, false)]
        [DataRow(3, 1, 1, true)]
        [DataRow(0, 0, 0, false)]
        [DataRow(0, -1, 0, false)]
        [DataRow(0, 0, -1, false)]
        public void ApisViewModel_ShowsNextPageLink(int total, int skipped, int taken, bool expected)
        {
            // act
            var model = new ApisViewModel(total, skipped, taken);

            // assert
            Assert.AreEqual(expected, model.ShowNextLink);
        }

        [TestMethod]
        [DataRow(0, 11, 5, 5)]
        [DataRow(0, 10, 5, 4)]
        [DataRow(0, 9, 5, 4)]
        [DataRow(0, 6, 5, 4)]
        public void ApisViewModel_NextPage_RoundsUp(int total, int skipped, int taken, int expected)
        {
            // act
            var model = new ApisViewModel(total, skipped, taken);

            // assert
            Assert.AreEqual(expected, model.NextPage);
        }

        [TestMethod]
        [DataRow(0, 10, 5, "/apis?skip=15&take=5")]
        [DataRow(0, 15, 10, "/apis?skip=25&take=10")]
        [DataRow(0, 0, 5, "/apis?skip=5&take=5")]
        public void ApisViewModel_NextPageLink_LinksToNextSet(int total, int skipped, int taken, string expected)
        {
            // act
            var model = new ApisViewModel(total, skipped, taken);

            // assert
            Assert.AreEqual(expected, model.NextPageLink);
        }
    }
}
