using TaxiGomel.Data;
using TaxiGomel.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace TaxiGomel.Services
{
    public class CachedEmployeesService : ICachedEmployeesService
    {
        private readonly TaxiGomelContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedEmployeesService(TaxiGomelContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        // получение списка емкостей из базы
        public IEnumerable<Employee> GetEmployees(int rowsNumber = 20)
        {
            return _dbContext.Employees.Take(rowsNumber).ToList();
        }

        // добавление списка емкостей в кэш
        public void AddEmployees(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Employee> Employees = _dbContext.Employees.Take(rowsNumber).ToList();
            if (Employees != null)
            {
                _memoryCache.Set(cacheKey, Employees, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            }

        }
        // получение списка емкостей из кэша или из базы, если нет в кэше
        public IEnumerable<Employee> GetEmployees(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Employee> Employees;
            if (!_memoryCache.TryGetValue(cacheKey, out Employees))
            {
                Employees = _dbContext.Employees.Take(rowsNumber).ToList();
                if (Employees != null)
                {
                    _memoryCache.Set(cacheKey, Employees,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return Employees;
        }
        public IEnumerable<Employee> GetEmployeesByAgeAndPosition(int Age, int PositionID)
        {
            return _dbContext.Employees.Where(e => e.Age >= Age).Where(e => e.PositionId == PositionID).ToList();
        }
    }
}

