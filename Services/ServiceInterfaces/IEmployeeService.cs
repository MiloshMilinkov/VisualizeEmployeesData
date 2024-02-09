using VisualizeEmployeesData.Models;

namespace VisualizeEmployeesData.Services.ServiceInterfaces
{
    public interface IEmployeeService
    {
        public Task<List<Employee>> GetEmployeesAsync();
        public Task<List<EmployeeHours>> GetEmployeeNameAndTotalHourse();
    }
}
