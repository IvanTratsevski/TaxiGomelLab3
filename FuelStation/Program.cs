using TaxiGomel.Data;
using TaxiGomel.Infrastructure;
using TaxiGomel.Middleware;
using TaxiGomel.Models;
using TaxiGomel.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Html;
using System.Data.SqlTypes;

namespace FuelStationT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            // внедрение зависимости для доступа к БД с использованием EF
            string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<TaxiGomelContext>(options => options.UseSqlServer(connection));

            // добавление кэширования
            services.AddMemoryCache();

            // добавление поддержки сессии
            services.AddDistributedMemoryCache();
            services.AddSession();

            // внедрение зависимости CachedCarsService
            services.AddScoped<ICachedCarsService, CachedCarsService>();
            services.AddScoped<ICachedCarModelsService, CachedCarModelsService>();
            services.AddScoped<ICachedEmployeesService, CachedEmployeesService>();
            services.AddScoped<ICachedPositionsService, CachedPositionsService>();

            //Использование MVC - отключено
            //services.AddControllersWithViews();
            var app = builder.Build();


            // добавляем поддержку статических файлов
            app.UseStaticFiles();

            // добавляем поддержку сессий
            app.UseSession();

            app.Map("/searchform1", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string key = "car_s";
                    CarSession car_s = context.Session.Get<CarSession>(key) ?? new CarSession(0, 0);
                    ICachedCarsService cachedCarsService = context.RequestServices.GetService<ICachedCarsService>();
                    ICachedCarModelsService cachedCarModelsService = context.RequestServices.GetService<ICachedCarModelsService>();
                    IEnumerable<Car> cars = cachedCarsService.GetCarsByPriceAndModel(car_s.MinPrice, car_s.CarModelID);
                    IEnumerable<CarModel> car_models = cachedCarModelsService.GetCarModels(20);
                    if (context.Request.Method == "POST")
                    {
                        car_s.MinPrice = Int32.Parse(context.Request.Form["MinPrice"]);
                        car_s.CarModelID = Int32.Parse(context.Request.Form["CarModelID"]);
                        context.Session.Set(key, car_s);
                    }

                    string strResponse = "<HTML><HEAD><TITLE>Searchform1</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><FORM action ='/searchform1' method = \"POST\"/ >" +
                    "Порог цены:<BR><INPUT type = 'number' name = 'MinPrice' value=" + car_s.MinPrice + ">";
                    strResponse += "<label for= \"CarModelID\"> Марка машины:</label>";
                    strResponse += "<select id=\"CarModelID\" name=\"CarModelID\"> ";
                    foreach (var model in car_models)
                    {
                        if (model.CarModelID == car_s.CarModelID)
                        {
                            strResponse += "<option value=\"" + model.CarModelID + "\" selected>" + model.ModelName + "</option>";
                        }
                        else
                        {
                            strResponse += "<option value=\"" + model.CarModelID + "\">" + model.ModelName + "</option>";
                        }

                    }
                    strResponse += "</select>";
                    strResponse += "<TABLE BORDER=1>";
                    strResponse += "<TR>";
                    strResponse += "<TH>ID машины</TH>";
                    strResponse += "<TH>Регистрационный номер</TH>";
                    strResponse += "<TH>Марка</TH>";
                    strResponse += "<TH>Номер кузова</TH>";
                    strResponse += "<TH>Номер двигателя</TH>";
                    strResponse += "<TH>Год выпуска</TH>";
                    strResponse += "<TH>Пробег</TH>";
                    strResponse += "<TH>Последний тех.осмотр</TH>";
                    strResponse += "<TH>Особые отметки</TH>";
                    strResponse += "</TR>";
                    foreach (var car in cars)
                    {
                        strResponse += "<TD>" + car.CarId + "</TD>";
                        strResponse += "<TD>" + car.RegistrationNumber + "</TD>";
                        strResponse += "<TD>" + car.Model.ModelName + "</TD>";
                        strResponse += "<TD>" + car.CarcaseNumber + "</TD>";
                        strResponse += "<TD>" + car.EngineNumber + "</TD>";
                        strResponse += "<TD>" + car.ReleaseYear + "</TD>";
                        strResponse += "<TD>" + car.Mileage + "</TD>";
                        strResponse += "<TD>" + car.LastTi + "</TD>";
                        strResponse += "<TD>" + car.SpecialMarks + "</TD>";
                    }
                    strResponse += "</TABLE>";
                    strResponse += "<BR><BR><INPUT type ='submit' value='Сохранить в Session'></FORM>";
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
                    await context.Response.WriteAsync(strResponse);
                });
            });
            app.Map("/searchform2", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string key = "cookied_employee";
                    EmployeeCookie emp_cook = new EmployeeCookie(0, 0);
                    CookieOptions options = new CookieOptions { Expires = DateTime.Now.AddDays(1) };
                    if (context.Request.Method == "POST")
                    {
                        emp_cook.Age = Int32.Parse(context.Request.Form["Age"]);
                        emp_cook.PositionID = Int32.Parse(context.Request.Form["PositionID"]);
                        context.Response.Cookies.Append(key, JsonConvert.SerializeObject(emp_cook), options);

                    }
                    else if (context.Request.Cookies.ContainsKey(key))
                    {
                        emp_cook = JsonConvert.DeserializeObject<EmployeeCookie>(context.Request.Cookies[key]);
                    }
                    ICachedEmployeesService cachedEmployeesService = context.RequestServices.GetService<ICachedEmployeesService>();
                    ICachedPositionsService cachedPositionsService = context.RequestServices.GetService<ICachedPositionsService>();
                    IEnumerable<Employee> employeesByAgePos = cachedEmployeesService.GetEmployeesByAgeAndPosition(emp_cook.Age, emp_cook.PositionID);
                    IEnumerable<Position> positions = cachedPositionsService.GetPositions(29);
                    string strResponse = "<HTML><HEAD><TITLE>Searchform2</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><FORM action ='/searchform2' method = \"POST\"/ >" +
                    "Порог возраста:<BR><INPUT type = 'text' name = 'Age' value=" + emp_cook.Age + ">";
                    strResponse += "<label for= \"PositionID\"> Марка машины:</label>";
                    strResponse += "<select id=\"PositionID\" name=\"PositionID\"> ";
                    foreach (var position in positions)
                    {
                        if (position.PositionId == emp_cook.PositionID)
                        {
                            strResponse += "<option value=\"" + position.PositionId + "\" selected>" + position.PositionName + "</option>";
                        }
                        else
                        {
                            strResponse += "<option value=\"" + position.PositionId + "\">" + position.PositionName + "</option>";
                        }

                    }
                    strResponse += "</select>";
                    strResponse += "<TABLE BORDER=1>";
                    strResponse += "<TR>";
                    strResponse += "<TH>Имя</TH>";
                    strResponse += "<TH>Фамилия</TH>";
                    strResponse += "<TH>Возраст</TH>";
                    strResponse += "<TH>Опыт</TH>";
                    strResponse += "</TR>";
                    foreach (var emp in employeesByAgePos)
                    {
                        strResponse += "<TR>";
                        strResponse += "<TD>" + emp.FirstName + "</TD>";
                        strResponse += "<TD>" + emp.LastName + "</TD>";
                        strResponse += "<TD>" + emp.Age + "</TD>";
                        strResponse += "<TD>" + emp.Experience + "</TD>";
                        strResponse += "</TR>";
                    }
                    strResponse += "</TABLE>";
                    strResponse += "<BR><BR><INPUT type ='submit' value='Сохранить в Cookie'><INPUT type ='submit' value='Показать'></FORM>";
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
                    await context.Response.WriteAsync(strResponse);
                });
            });
            //Запоминание в Session значений, введенных в форме
          



            //Запоминание в Сookies значений, введенных в форме
            //..


            // Вывод информации о клиенте
            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Формирование строки для вывода 
                    string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Информация:</H1>";
                    strResponse += "<BR> Сервер: " + context.Request.Host;
                    strResponse += "<BR> Путь: " + context.Request.PathBase;
                    strResponse += "<BR> Протокол: " + context.Request.Protocol;
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
                    // Вывод данных
                    await context.Response.WriteAsync(strResponse);
                });
            });

            // Вывод кэшированной информации из таблицы базы данных
            app.Map("/Cars", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //обращение к сервису
                    ICachedCarsService cachedCarsService = context.RequestServices.GetService<ICachedCarsService>();
                    IEnumerable<Car> cars = cachedCarsService.GetCars("cars20");
                    string strResponse = "<HTML><HEAD><TITLE>Машины</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Машины</H1>" +
                    "<TABLE BORDER=1>";
                    strResponse += "<TR>";
                    strResponse += "<TH>ID машины</TH>";
                    strResponse += "<TH>Регистрационный номер</TH>";
                    strResponse += "<TH>Марка</TH>";
                    strResponse += "<TH>Номер кузова</TH>";
                    strResponse += "<TH>Номер двигателя</TH>";
                    strResponse += "<TH>Год выпуска</TH>";
                    strResponse += "<TH>Пробег</TH>";
                    strResponse += "<TH>Последний тех.осмотр</TH>";
                    strResponse += "<TH>Особые отметки</TH>";
                    strResponse += "</TR>";
                    foreach (var car in cars)
                    {
                        strResponse += "<TR>";
                        strResponse += "<TD>" + car.CarId + "</TD>";
                        strResponse += "<TD>" + car.RegistrationNumber + "</TD>";
                        strResponse += "<TD>" + car.Model.ModelName + "</TD>";
                        strResponse += "<TD>" + car.CarcaseNumber + "</TD>";
                        strResponse += "<TD>" + car.EngineNumber + "</TD>";
                        strResponse += "<TD>" + car.ReleaseYear + "</TD>";
                        strResponse += "<TD>" + car.Mileage + "</TD>";
                        strResponse += "<TD>" + car.LastTi + "</TD>";
                        strResponse += "<TD>" + car.SpecialMarks + "</TD>";
                        strResponse += "</TR>";
                    }
                    strResponse += "</TABLE>";
                    strResponse += "<BR><A href='/'>Главная</A></BR>";
                    strResponse += "</BODY></HTML>";

                    // Вывод данных
                    await context.Response.WriteAsync(strResponse);
                });
            });



            // Стартовая страница и кэширование данных таблицы на web-сервере
            app.Run((context) =>
            {
                //обращение к сервису
                ICachedCarsService cachedCarsService = context.RequestServices.GetService<ICachedCarsService>();
                cachedCarsService.AddCars("cars20");
                string strResponse = "<HTML><HEAD><TITLE>Машины</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>Главная</H1>";
                strResponse += "<H2>Данные записаны в кэш сервера</H2>";
                strResponse += "<BR><A href='/'>Главная</A></BR>";
                strResponse += "<BR><A href='/info'>Информация о подключении</A></BR>";
                strResponse += "<BR><A href='/Cars'>Автопарк</A></BR>";
                strResponse += "<BR><A href='/searchform1'>Поиск машин</A></BR>";
                strResponse += "<BR><A href='/searchform2'>Поиск сотрудников</A></BR>";
                strResponse += "</BODY></HTML>";

                return context.Response.WriteAsync(strResponse);

            });

            //Использование MVC - отключено
            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.Run();
        }
    }
}