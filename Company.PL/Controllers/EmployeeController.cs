using System.Reflection.Metadata;
using System.Threading.Tasks;
using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.Dtos;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

                // Dictionary    : 3 Properties
                // 1. ViewData   : Transfer Extra Information From Controller (Action) To View

                //ViewData["Message"] = "Hello From ViewData";

                // 2. ViewBag    : Transfer Extra Information From Controller (Action) To View

                //ViewBag.Message = "Hello From ViewBag";

                // 3. TempData   :

                return View(employees);
        }

        public async Task<IActionResult> Search(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

            // Dictionary    : 3 Properties
            // 1. ViewData   : Transfer Extra Information From Controller (Action) To View

            //ViewData["Message"] = "Hello From ViewData";

            // 2. ViewBag    : Transfer Extra Information From Controller (Action) To View

            //ViewBag.Message = "Hello From ViewBag";

            // 3. TempData   :

            return PartialView("/Views/Employee/EmployeePartialView/EmployeesTablePartialView.cshtml", employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
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
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    TempData["Message"] = "Employee Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, Message = $"Employee with id : {id} is not found" });
            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string viewName = "Edit")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            var deparments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = deparments;
            if (employee is null) return NotFound(new { statusCode = 404, Message = $"Employee with id : {id} is not found" });
            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(viewName, dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model, string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {
                if (model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }

                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;

                _unitOfWork.EmployeeRepository.Update(employee);
                var Count = await _unitOfWork.CompleteAsync();
                if(Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(viewName, model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Edit(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Delete(employee);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    if (model.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }


    }
}
