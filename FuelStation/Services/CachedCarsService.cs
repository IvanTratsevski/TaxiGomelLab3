using TaxiGomel.Data;
using TaxiGomel.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace TaxiGomel.Services
{
    public class CachedCarsService : ICachedCarsService
    {
        private readonly TaxiGomelContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedCarsService(TaxiGomelContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        // получение списка емкостей из базы
        public IEnumerable<Car> GetCars(int rowsNumber = 20)
        {
            return _dbContext.Cars.Include(с => с.Model).Take(rowsNumber).ToList();
        }

        // добавление списка емкостей в кэш
        public void AddCars(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Car> Cars = _dbContext.Cars.Include(с => с.Model).Take(rowsNumber).ToList();
            if (Cars != null)
            {
                _memoryCache.Set(cacheKey, Cars, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            }

        }
        // получение списка емкостей из кэша или из базы, если нет в кэше
        public IEnumerable<Car> GetCars(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Car> Cars;
            if (!_memoryCache.TryGetValue(cacheKey, out Cars))
            {
                Cars = _dbContext.Cars.Include(с => с.Model).Take(rowsNumber).ToList();
                if (Cars != null)
                {
                    _memoryCache.Set(cacheKey, Cars,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return Cars;
        }
        public IEnumerable<Car> GetCarsByPriceAndModel(int price, int CarModelID)
        {
            return _dbContext.Cars.Include(car => car.Model).Where(car => car.Model.Price >= price).Where(car => car.CarModelID == CarModelID).ToList();
        }
    }
}

