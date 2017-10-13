using Xunit;
using Moq;
using BDSA2017.Assignment07.Models;
using Microsoft.AspNetCore.Mvc;
using BDSA2017.Assignment07.Web.Properties;
using System.Collections.Generic;
using System.Linq;

namespace BDSA2017.Assignment07.Web.Tests
{
    public class TracksControllerTests
    {
        [Fact]
        public async void getGivenNoExistingIDReturnsNotFound()
        {
            var mockRepository = new Mock<ITrackRepository>();
            mockRepository.Setup(a => a.Find(42)).ReturnsAsync (default(TrackDTO));
            var controller = new TracksController(mockRepository.Object);
            var response = await controller.Get(42);
            Assert.IsType<NotFoundResult>(response);
        }
        [Fact]
        public async void getGivenExistingIDReturnsTrack()
        {
            var TrackDTO = new TrackDTO() { Id = 42 }; 
            var mockRepository = new Mock<ITrackRepository>();
            mockRepository.Setup(a => a.Find(42)).ReturnsAsync(TrackDTO);
            var controller = new TracksController(mockRepository.Object);
            var response = await controller.Get(42) as OkObjectResult;
            Assert.Equal(TrackDTO,response.Value);
        }
        [Fact]
        public void getAllWithItemsReturnsOk()
        {
            var list = new List<TrackDTO>() { new TrackDTO() { Id = 1221 } }.AsQueryable();
            var mockRepository = new Mock<ITrackRepository>();
            mockRepository.Setup(a => a.Read()).Returns(list);
            var controller = new TracksController(mockRepository.Object);
            var response = controller.Get() as OkObjectResult;
            Assert.Equal(list, response.Value);

        }
        [Fact]
        public  void getAllWithNoItemsReturnsNoContent()
        {
            var mockRepository = new Mock<ITrackRepository>();
            mockRepository.Setup(a => a.Read()).Returns(default(IQueryable<TrackDTO>));
            var controller = new TracksController(mockRepository.Object);
            var response = controller.Get();
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async void postWithBadTrackReturnsBadRequest()
        {
            var track = new TrackCreateDTO() {  };
            var mockRepository = new Mock<ITrackRepository>();
            var controller = new TracksController(mockRepository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var response = await controller.Post(track);
            Assert.IsType<BadRequestObjectResult>(response);
        }
        [Fact]
        public async void postWithGoodTrackReturnsOK()
        {
            var track = new TrackCreateDTO() {Name ="eat", LengthInMeters =100, MaxCars = 50 };
            var mockRepository = new Mock<ITrackRepository>();
            mockRepository.Setup(a => a.Create(track)).ReturnsAsync(1231);
            var controller = new TracksController(mockRepository.Object);
            var response = await controller.Post(track) as CreatedAtActionResult;

            Assert.Equal("Get",response.ActionName);
            Assert.Equal(1231, response.RouteValues["id"]);

        }
        [Fact]
        public async void putWithBadModelState()
        {
            var track = new TrackUpdateDTO() { };
            var mockRepository = new Mock<ITrackRepository>();
            var controller = new TracksController(mockRepository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var response = await controller.Put(0,track);
            Assert.IsType<BadRequestObjectResult>(response);
        }
        [Fact]
        public async void putWithNotFound()
        {
            var track = new TrackUpdateDTO() { Id = 12121};
            var mockRepository = new Mock<ITrackRepository>();
            mockRepository.Setup(a => a.Update(track)).ReturnsAsync(false);
            var controller = new TracksController(mockRepository.Object);
            var response = await controller.Put(12121, track) ;
            
            Assert.IsType<NotFoundResult>(response);
        }
        [Fact]
        public async void putSucces()
        {
            var track = new TrackUpdateDTO() { Id = 12121 };
            var mockRepository = new Mock<ITrackRepository>();
            mockRepository.Setup(a => a.Update(track)).ReturnsAsync(true);
            var controller = new TracksController(mockRepository.Object);
            var response = await controller.Put(12121, track) as OkObjectResult;

            Assert.Equal(track,response.Value);
        }
        [Fact]
        public async void deleteFailure()
        {
            var mockRepository = new Mock<ITrackRepository>();
            mockRepository.Setup(a => a.Delete(0)).ReturnsAsync(false);
            var controller = new TracksController(mockRepository.Object);
            var response = await controller.Delete(0);

            Assert.IsType<NotFoundResult>(response);
        }
        [Fact]
        public async void deleteSuccess()
        {
            var mockRepository = new Mock<ITrackRepository>();
            mockRepository.Setup(a => a.Delete(0)).ReturnsAsync(true);
            var controller = new TracksController(mockRepository.Object);
            var response = await controller.Delete(0);
            
            Assert.IsType<OkResult>(response);
        }





    }
}
