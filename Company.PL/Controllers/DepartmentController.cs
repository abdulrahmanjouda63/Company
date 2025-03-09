using Company.BLL.Interfaces;
using Company.BLL.Repositories;
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
    }
}
