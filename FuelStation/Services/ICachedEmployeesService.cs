using TaxiGomel.Models;
using System.Collections.Generic;

namespace TaxiGomel.Services
{
    public interface ICachedEmployeesService
    {
        public IEnumerable<Employee> GetEmployees(int rowsNumber = 20);
        public void AddEmployees(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Employee> GetEmployees(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Employee> GetEmployeesByAgeAndPosition(int Age, int PositionID);
    }
}
