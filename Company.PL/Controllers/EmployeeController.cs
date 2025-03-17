using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _employeeRepository.GetAll();
            }
            else {
                employees = _employeeRepository.GetByName(SearchInput);
            }

                // Dictionary    : 3 Properties
                // 1. ViewData   : Transfer Extra Information From Controller (Action) To View

                //ViewData["Message"] = "Hello From ViewData";

                // 2. ViewBag    : Transfer Extra Information From Controller (Action) To View

                //ViewBag.Message = "Hello From ViewBag";

                // 3. TempData   :

                return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentRepository.GetAll();
            ViewData["Departments"] = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var employee = new Employee()
                    //{
                    //Name = model.Name,
                    //Address = model.Address,
                    //Age = model.Age,
                    //Email = model.Email,
                    //Phone = model.Phone,
                    //Salary = model.Salary,
                    //IsActive = model.IsActive,
                    //IsDeleted = model.IsDeleted,
                    //HiringDate = model.HiringDate,
                    //CreateAt = model.CreateAt,
                    //DepartmentId = model.DepartmentId
                    //};

                    var employee = _mapper.Map<Employee>(model);
                    var Count = _employeeRepository.Add(employee);
                    if (Count > 0)
                    {
                        TempData["Message"] = "Employee Added Successfully";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(model);
        }

        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { Message = $"Employee with id : {id} is not found" });
            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _departmentRepository.GetAll();
            ViewData["Departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { Message = $"Employee with id : {id} is not found" });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest();
                var Count = _employeeRepository.Update(model);
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = _employeeRepository.Get(id.Value);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var employee = _employeeRepository.Get(id);
            if (employee == null)
                return NotFound();

            _employeeRepository.Delete(employee);
            return RedirectToAction(nameof(Index));
        }


    }
}
