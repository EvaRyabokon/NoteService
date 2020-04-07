using Microsoft.AspNetCore.Mvc;
using Moq;
using NotesService.Controllers;
using NotesService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NotesServiceTests
{
    public class NoteControllerTests
    {
        [Fact]
        public void ListEmptySearchTest()
        {
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(GetTestNotes());
            var controller = new NoteController(mockRepo.Object);
           
            var result = controller.List("").Result;
            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Note>>(viewResult.Model);
            IEnumerable<Note> difference = model.Except(GetTestNotes()); 
            Assert.Empty(difference);
        }

        [Fact]
        public void ListChecktringSearchTest()
        {
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.GetBySubstring("smth")).ReturnsAsync(GetFilteredNotes("smth"));
            var controller = new NoteController(mockRepo.Object);

            var result = controller.List("smth").Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Note>>(viewResult.Model);
            IEnumerable<Note> difference = model.Except(GetFilteredNotes("smth"));
            Assert.Empty(difference);
        }

        [Fact]
        public void CreateShouldReturnRedirectToActionListTest()
        {
            var note = new Note { Id = 1, Title = "todo", Content = "do smth" };
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.Create(note));
            var controller = new NoteController(mockRepo.Object);

            var result = controller.Create(note).Result;

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(controller.List), redirectResult.ActionName);
        }

        [Fact]
        public void CreateShouldBeCalledOnceTest()
        {
            var note = new Note { Id = 1, Title = "todo", Content = "do smth" };
            var mockRepo = new Mock<INoteRepository>();
            var controller = new NoteController(mockRepo.Object);

            var result = controller.Create(note).Result;

            mockRepo.Verify(repo => repo.Create(note), Times.Once());
        }

        [Fact]
        public void CreateShouldHaveNoViewModelTest()
        {
            var mockRepo = new Mock<INoteRepository>();
            var controller = new NoteController(mockRepo.Object);

            var result = controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task DeleteShouldReturnRedirectToActionList()
        {
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.DeleteById(1)); 
            var controller = new NoteController(mockRepo.Object);

            var result = await controller.Delete(1);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(controller.List), redirectResult.ActionName);
        }

        [Fact]
        public void DeleteShouldBeCalledOnceTest()
        {
            var mockRepo = new Mock<INoteRepository>();
            var controller = new NoteController(mockRepo.Object);

            var result = controller.Delete(1).Result;

            mockRepo.Verify(repo => repo.DeleteById(1), Times.Once());
        }

        [Fact]
        public void EditShouldReturnRedirectToActionListTest()
        {
            var note = new Note { Id = 1, Title = "todo", Content = "do smth" };
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.Update(note));
            var controller = new NoteController(mockRepo.Object);

            var result = controller.Edit(note).Result;

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(controller.List), redirectResult.ActionName);
        }

        [Fact]
        public void EditShouldBeCalledOnceTest()
        {
            var note = new Note { Id = 1, Title = "todo", Content = "do smth" };
            var mockRepo = new Mock<INoteRepository>();
            var controller = new NoteController(mockRepo.Object);

            var result = controller.Edit(note).Result;

            mockRepo.Verify(repo => repo.Update(note), Times.Once());
        }

        [Fact]
        public async Task EditShouldReturnNotFoundIfUnexistentIdPassedTest()
        {
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.GetById(0)).ReturnsAsync((Note)null);
            var controller = new NoteController(mockRepo.Object);

            var result = await controller.Edit(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditShouldReturnCorrectNoteTest()
        {
            var note = new Note { Id = 1, Title = "todo", Content = "do smth" };
            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync(note);
            var controller = new NoteController(mockRepo.Object);

            var result = await controller.Edit(note.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsAssignableFrom<Note>(viewResult.ViewData.Model);
            Assert.Equal(note, viewModel);
        }

        private List<Note> GetTestNotes()
        {
            var notes = new List<Note>
            {
                new Note { Id=1, Title="todo", Content = "do smth"},
                new Note { Id=2, Title="cook", Content = "do it"},
                new Note { Id=3, Title="rest", Content = "do exercises"},
                new Note { Id=4, Title="work", Content = "do smth"},
            };
            return notes;
        }

        private List<Note> GetFilteredNotes(string searchString)
        {
            var notes = new List<Note>
            {
                new Note { Id=1, Title="todo", Content = "do smth"},
                new Note { Id=2, Title="cook", Content = "do it"},
                new Note { Id=3, Title="rest", Content = "do exercises"},
                new Note { Id=4, Title="work", Content = "do smth"},
            };

            return notes.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper())
            || s.Content.ToUpper().Contains(searchString.ToUpper())).ToList();
        }
    }
}
