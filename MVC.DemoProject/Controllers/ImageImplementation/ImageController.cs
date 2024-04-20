using Microsoft.AspNetCore.Mvc;
using MVC.DemoProject.Data;
using MVC.DemoProject.Models;
using MVC.DemoProject.Models.ImageImplementation;
using System.Drawing.Text;

namespace MVC.DemoProject.Controllers.ImageImplementation
{
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHost;

        public ImageController(ApplicationDbContext dbContext, IWebHostEnvironment webHost)
        {
            _dbContext = dbContext;
            _webHost = webHost;
        }
        public async Task<IActionResult> Index()
        {
            var img = _dbContext.Images.ToList();
            return View(img);
        }

        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(Image image)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadImage(image);

                var newImage = new Image()
                {
                    Description = image.Description,
                    ImagePath = uniqueFileName,
                };

                await _dbContext.Images.AddAsync(newImage);
                await _dbContext.SaveChangesAsync();
                TempData["insert_success"] = "Image Inserted Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                // Handle validation errors
                // Extract error messages from ModelState
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Pass error messages to the view
                ViewData["ErrorMessages"] = errorMessages;

                TempData["insert_failed"] = "Failed to add data";
                return View();
            }
        }

        private string UploadImage(Image img)
        {
            string uniqueFileName = null;

            if (img.ImageFile != null && img.ImageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(_webHost.WebRootPath, "Uploads/Images");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + img.ImageFile.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    img.ImageFile.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var imgData = await _dbContext.Images.FindAsync(id);

            return View(imgData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Image model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var imgData = _dbContext.Images.FirstOrDefault(x => x.Id == id);
                string uniqueFileName=string.Empty;

                if(imgData.ImagePath!= null)
                {
                    string filePath = Path.Combine(_webHost.WebRootPath, "Uploads/Images", imgData.ImagePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    };
                }
                uniqueFileName = UploadImage(model);

                imgData.Description = model.Description;

                if (model.ImageFile != null)
                {
                    imgData.ImagePath = uniqueFileName;
                }


                _dbContext.Images.Update(imgData);
                await _dbContext.SaveChangesAsync();

                TempData["update_success"] = "Images Data updated  Successfully";
                return RedirectToAction("Index");
            }


            TempData["insert_failed"] = "faild to add student";
            return View(model);
        }



        public async Task<IActionResult> Delete(int id)
        {
            if (id == null | _dbContext.Images == null)
            {
                return NotFound();
            }
            //var img = await _studentDb.Students.FirstOrDefaultAsync(x=>x.id==id);
            var img = _dbContext.Images.FirstOrDefault(x => x.Id == id);

            if (img == null)
            {
                return NotFound();
            }
            return View(img);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var imgData = await _dbContext.Images.FindAsync(id);

            //var imgId= _dbContext.Images.Where(e=>e.Id==id).SingleOrDefault();
            if (imgData == null)
            {
                return NotFound();
            }

            string deleteFromFolder = Path.Combine(_webHost.WebRootPath, "Uploads/Images");
            string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, imgData.ImagePath);
            if(currentImage != null)
            {
                if (System.IO.File.Exists(currentImage))
                {
                    System.IO.File.Delete(currentImage);
                };
            };
            _dbContext.Images.Remove(imgData);
            await _dbContext.SaveChangesAsync();
            TempData["delete_success"] = "image Deleted Successfully";

            return RedirectToAction("Index");
        }

    }
}
