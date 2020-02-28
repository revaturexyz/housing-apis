using Revature.Lodging.DataAccess.Entities;
using System;
using Xunit;

namespace Revature.Lodging.Tests
{
    public class EntityTests
    {
        // <summary>
        // Test Amenity entity to ensure it is grabbing the correct information - KAT 2/27/20
        // </summary>
        [Fact]
        public void AmenityTest()
        {
            var aId = Guid.NewGuid();
            var amenity = new Amenity
            {
              AmenityId = aId,
              AmenityType = "Test",
              Description = "describe"
            };

            Assert.Equal(aId, amenity.AmenityId);
            Assert.Equal("Test", amenity.AmenityType);
            Assert.Equal("describe", amenity.Description);
        }

        // <summary>
        // Test AmenityComplex entity to ensure it is grabbing the correct information - KAT 2/27/20
        // </summary>
        [Fact]
        public void AmenityComplexTest()
        {
            var acId = Guid.NewGuid();
            var aId = Guid.NewGuid();
            var cId = Guid.NewGuid();

            var amenityComplex = new AmenityComplex
            {
                AmenityComplexId = acId,
                AmenityId = aId,
                ComplexId = cId
            };

            Assert.Equal(acId, amenityComplex.AmenityComplexId);
            Assert.Equal(aId, amenityComplex.AmenityId);
            Assert.Equal(cId, amenityComplex.ComplexId);
        }

        // <summary>
        // Test AmenityFloorPlan entity to ensure it is grabbing the correct information - KAT 2/27/20
        // </summary>
        [Fact]
        public void AmenityFloorPlanTest()
        {
            var afId = Guid.NewGuid();
            var aId = Guid.NewGuid();
            var fId = Guid.NewGuid();

            var amenityFP = new AmenityFloorPlan
            {
                AmenityFloorPlanID = afId,
                AmenityID = aId,
                FloorPlanID = fId
            };

            Assert.Equal(afId, amenityFP.AmenityFloorPlanID);
            Assert.Equal(aId, amenityFP.AmenityID);
            Assert.Equal(fId, amenityFP.FloorPlanID);
        }

        // <summary>
        // Test AmenityRoom entity to ensure it is grabbing the correct information - KAT 2/27/20
        // </summary>
        [Fact]
        public void AmenityRoomTest()
        {
            var arId = Guid.NewGuid();
            var aId = Guid.NewGuid();
            var rId = Guid.NewGuid();

            var amenityRoom = new AmenityRoom
            {
                AmenityRoomId = arId,
                AmenityId = aId,
                RoomId = rId
            };

            Assert.Equal(arId, amenityRoom.AmenityRoomId);
            Assert.Equal(aId, amenityRoom.AmenityId);
            Assert.Equal(rId, amenityRoom.RoomId);
        }

        /// <summary>
        /// Test Complex entity to ensure it is grabbing the correct information - KAT 2/27/20
        /// </summary>
        [Fact]
        public void ComplexTest()
        {
            var aId = Guid.NewGuid();
            var pId = Guid.NewGuid();

            var complex = new Complex
            {
              ComplexId = aId,
              ComplexName = "Test",
              ContactNumber = "1234567890",
              ProviderId = pId
            };

            Assert.Equal(aId, complex.ComplexId);
            Assert.Equal(pId, complex.ProviderId);
            Assert.Equal("Test", complex.ComplexName);
            Assert.Equal("1234567890", complex.ContactNumber);
        }

        /// <summary>
        /// Test FloorPlan entity to ensure it is grabbing the correct information - KAT 2/27/20
        /// </summary>
        [Fact]
        public void FloorPlanTest()
        {
            var fId = Guid.NewGuid();
            var cId = Guid.NewGuid();

            var fp = new FloorPlan
            {
                FloorPlanID = fId,
                FloorPlanName = "Floor plan A",
                NumberBeds = 1,
                RoomTypeID = 1,
                ComplexID = cId
            };

            Assert.Equal(fId, fp.FloorPlanID);
            Assert.Equal("Floor plan A", fp.FloorPlanName);
            Assert.Equal(1, fp.NumberBeds);
            Assert.Equal(1, fp.RoomTypeID);
            Assert.Equal(cId, fp.ComplexID);
        }

        /// <summary>
        /// Test Gender entity to ensure it is grabbing the correct information - KAT 2/27/20
        /// </summary>
        [Fact]
        public void GenderTest()
        {
            var gender = new Gender
            {
                GenderID = 1,
                Type = "Non-binary"
            };

            Assert.Equal(1, gender.GenderID);
            Assert.Equal("Non-binary", gender.Type);
        }

        /// <summary>
        /// Test Room entity to ensure it is grabbing the correct information - KAT 2/27/20
        /// </summary>
        [Fact]
        public void RoomTest()
        {
          var cId = Guid.NewGuid();
          var start = DateTime.Parse("2019/1/1");
          var end = DateTime.Parse("2020/1/1");
          var room = new Room
          {
              RoomNumber = "1234",
              ComplexId = cId,
              NumberOfBeds = 3,
              RoomType = new RoomType { Type = "apartment" },
              LeaseStart = start,
              LeaseEnd = end
          };

          Assert.Equal("1234", room.RoomNumber);
          Assert.Equal(cId, room.ComplexId);
          Assert.Equal(3, room.NumberOfBeds);
          Assert.Equal("apartment", room.RoomType.Type);
          Assert.Equal(DateTime.Parse("2019/1/1"), room.LeaseStart);
          Assert.Equal(DateTime.Parse("2020/1/1"), room.LeaseEnd);
        }

        [Fact]
        public void RoomTypeTest()
        {
           var roomType = new RoomType
           {
              RoomTypeId = 1,
              Type = "Dormitory"
           };

           Assert.Equal(1, roomType.RoomTypeId);
           Assert.Equal("Dormitory", roomType.Type);
        }
    }
}
