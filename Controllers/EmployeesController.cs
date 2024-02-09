using Microsoft.AspNetCore.Mvc;
using VisualizeEmployeesData.Services.ServiceInterfaces;

namespace VisualizeEmployeesData.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> GetEmployeeNameAndTotalHourse()
        {
            var data = await _employeeService.GetEmployeeNameAndTotalHourse();
            return View(data);
        }
    }
}
