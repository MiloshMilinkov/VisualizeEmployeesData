using VisualizeEmployeesData.Models;
using VisualizeEmployeesData.Services.ServiceInterfaces;
using Newtonsoft.Json;

namespace VisualizeEmployeesData.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string apiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";
        private readonly EmployeeDataChartService _employeeDataChartService;
        public EmployeeService(EmployeeDataChartService employeeDataChartService) { 
            _employeeDataChartService = employeeDataChartService;
        }
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            using(var client = new HttpClient())
            {
                var json = await client.GetStringAsync(apiUrl);
                var employees = JsonConvert.DeserializeObject<List<Employee>>(json);
                var filteredEmployees = employees.Where(e => !string.IsNullOrEmpty(e.EmployeeName));
                return employees;
                
            }
        }

        public async Task<List<EmployeeHours>> GetEmployeeNameAndTotalHourse()
        {
            var employees = await GetEmployeesAsync();

            var employeeData =  employees.Where(e => !string.IsNullOrWhiteSpace(e.EmployeeName)).GroupBy(e => e.EmployeeName)
                        .Select(group => new EmployeeHours
                        {
                            EmployeeName = group.Key,
                            TotalHours = group.Sum(e => (e.EndTimeUtc - e.StarTimeUtc).TotalHours)
                        }).ToList();
            _employeeDataChartService.GeneratePieChartImage(employeeData);


            return employeeData;
        }
    }
}
