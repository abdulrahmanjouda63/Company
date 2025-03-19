using System.Threading.Tasks;
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
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        // ASK CLR Create Object From DepartmentRepository
        public DepartmentController(/*IDepartmentRepository departmentRepository*/IUnitOfWork unitOfWork)
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }
        [HttpGet] //GET: Department/Index
        public async Task<IActionResult> Index()
        {

            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");

            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department == null) return NotFound(new {statusCode = 404, message = $"department with Id: {id} Was No Found"});

            return View(department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");

            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department == null) return NotFound(new { statusCode = 404, message = $"department with Id: {id} Was No Found" });

            var dto = new CreateDepartmentDto()
            {
                Name = department.Name,
                Code = department.Code,
                CreateAt = department.CreateAt
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int? id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Id = (int)id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                _unitOfWork.DepartmentRepository.Update(department);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");

            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department == null) return NotFound(new { statusCode = 404, message = $"department with Id: {id} Was No Found" });

            var dto = new CreateDepartmentDto()
            {
                Name = department.Name,
                Code = department.Code,
                CreateAt = department.CreateAt
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Id = (int)id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                _unitOfWork.DepartmentRepository.Delete(department);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(model);
        }
    }
}
