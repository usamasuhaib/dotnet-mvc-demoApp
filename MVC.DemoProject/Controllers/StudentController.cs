
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MVC.DemoProject.Data;
using MVC.DemoProject.Models;

namespace MVC.DemoProject.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult StudentsList()
        {

            
            var students= _dbContext.Students.ToList();
            return View(students);
        }

        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> AddStudent(Student student)
        {
            if(ModelState.IsValid)
            {
               await _dbContext.Students.AddAsync(student);
               await _dbContext.SaveChangesAsync();
                TempData["insert_success"] = "Student Added Successfully";
                return RedirectToAction("StudentsList");
            }


            TempData["insert_failed"] = "faild to add student";
            return View();

        }


        public async Task< IActionResult> EditStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var stdData =await _dbContext.Students.FindAsync(id);

            return View(stdData);
        }

        [HttpPost]
        public async Task< IActionResult> EditStudent(int? id, Student std)
        {
            if (id != std.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Students.Update(std);
                await _dbContext.SaveChangesAsync();

                TempData["update_success"] = "Students Data updated  Successfully";
                return RedirectToAction("StudentsList");
            }


            TempData["insert_failed"] = "faild to add student";
            return View(std);
        }



        public async Task< IActionResult> DeleteStudent(int? Id)
        {
            if(Id== null| _dbContext.Students == null)
            {
                return NotFound();
            }
            //var stdData = await _studentDb.Students.FirstOrDefaultAsync(x=>x.id==id);
            var studentData= _dbContext.Students.FirstOrDefault(x=>x.Id==Id);

            if(studentData == null)
            {
                return NotFound();
            }
            return View(studentData);
        }

        [HttpPost]
        public async Task< IActionResult> DeleteConfirm(int Id)
        {
            var stdData = await _dbContext.Students.FindAsync(Id);
            if (stdData == null)
            {
                return NotFound();
            }
            _dbContext.Students.Remove(stdData);
          await _dbContext.SaveChangesAsync();
            TempData["delete_success"] = "Student Deleted Successfully";

            return RedirectToAction("StudentsList");
        }

    }
}
