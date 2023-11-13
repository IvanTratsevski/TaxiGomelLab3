using TaxiGomel.Data;
using TaxiGomel.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace TaxiGomel.Services
{
    public class CachedCarModelsService : ICachedCarModelsService
    {
        private readonly TaxiGomelContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedCarModelsService(TaxiGomelContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        // получение списка емкостей из базы
        public IEnumerable<CarModel> GetCarModels(int rowsNumber = 20)
        {
            return _dbContext.CarModels.Take(rowsNumber).ToList();
        }

        // добавление списка емкостей в кэш
        public void AddCarModels(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<CarModel> CarModels = _dbContext.CarModels.Take(rowsNumber).ToList();
            if (CarModels != null)
            {
                _memoryCache.Set(cacheKey, CarModels, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            }

        }
        // получение списка емкостей из кэша или из базы, если нет в кэше
        public IEnumerable<CarModel> GetCarModels(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<CarModel> CarModels;
            if (!_memoryCache.TryGetValue(cacheKey, out CarModels))
            {
                CarModels = _dbContext.CarModels.Take(rowsNumber).ToList();
                if (CarModels != null)
                {
                    _memoryCache.Set(cacheKey, CarModels,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return CarModels;
        }

    }
}

