using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    // MVC Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        // ASK CLR Create Object From DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        [HttpGet] //GET: Department/Index
        public IActionResult Index()
        {

            var departments = _departmentRepository.GetAll();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                var Count = _departmentRepository.Add(department);
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return NotFound();

            var Department = _departmentRepository.Get(id.Value);
            if (Department == null)
                return NotFound();

            return View(Department);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, Department department)
        {
            if (id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _departmentRepository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(department);
                }
            }
            return View(department);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int? id, Department department)
        {
            if (id != department.Id)
                return BadRequest();

            try
            {
                _departmentRepository.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(department);
            }

        }
    }
}
