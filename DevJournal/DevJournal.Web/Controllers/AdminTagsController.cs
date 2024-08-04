using DevJournal.Web.Data;
using DevJournal.Web.Models.Domain;
using DevJournal.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace DevJournal.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly DevjournalDbContext devjournalDbContext;

        public AdminTagsController(DevjournalDbContext devjournalDbContext)
        {
            this.devjournalDbContext = devjournalDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag Domain Model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            // Non Asynchronous call
            //devjournalDbContext.Tags.Add(tag);
            //devjournalDbContext.SaveChanges();

            await devjournalDbContext.Tags.AddAsync(tag);
            await devjournalDbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {

            // Use the dbContext to read the tags
            var tags = await devjournalDbContext.Tags.ToListAsync();

            return View(tags); 
        
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // 1st method
            // var tag = devjournalDbContext.Tags.Find(id);

            // 2nd method
            var tag = await devjournalDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var existingTag = await devjournalDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // Save the changes
                devjournalDbContext.SaveChanges();

                // Show success notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }


            // Show error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await devjournalDbContext.Tags.FindAsync(editTagRequest.Id);

            if (tag != null)
            {
                devjournalDbContext.Tags.Remove(tag);
                await devjournalDbContext.SaveChangesAsync();

                // Show success notification
                return RedirectToAction("List");
            }

            // Show error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

    }
}
