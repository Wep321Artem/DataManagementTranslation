﻿using DataManagementTranslation.Models;
using DataManagementTranslation.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace DataManagementTranslation.Controllers
{
    public class HomeController : Controller
    {

        private readonly ClientRepository _ClientRepository;

        public HomeController(ClientRepository clientRepository)
        {
            _ClientRepository = clientRepository;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var clients = await _ClientRepository.GetAsync();
                return View(clients);
            }
            catch (SqlException ex) { TempData["ErrorMassage"] = "Ошибка при подключении к Базам данных"; return View(new List<Clients>());}

        }



        public async Task AddIndataBaseAsync(List<Clients> clients)
        {
            foreach (var client in clients)
            {
                await _ClientRepository.AddAsync(
                    client.CardCode,
                    client.LastName,
                    client.SurName,
                    client.FirstName,
                    client.PhoneMobile,
                    client.Email,
                    client.GenderId,
                    client.Birthday,
                    client.City,
                    client.Pincode,
                    client.Bonus,
                    client.Turnover);
            }

        }


        [HttpPost]
        public async Task<IActionResult> UploadXLSX(IFormFile file)
        {
            var clientsJson = HttpContext.Session.GetString("ClientsToUpload");
            
            if (!string.IsNullOrEmpty(clientsJson))
            {
                var clients = JsonConvert.DeserializeObject<List<Clients>>(clientsJson);

                
                await AddIndataBaseAsync(clients);

                ////очистка сессии
                HttpContext.Session.Remove("ClientsToUpload");

                TempData["SuccessMessage"] = $"Успешно загружено {clients.Count} записей в базу данных.";
            }
            else
            {
                try
                {
                    if (file != null)
                    {
                        var clients = await _ClientRepository.GetDataByXLSXAsync(file);
                        await AddIndataBaseAsync(clients);
                        TempData["SuccessMessage"] = $"Успешно загружено {clients.Count} записей в базу данных.";
                    }
                    else { TempData["Errormessage"] = "Внимание! Файл пустой"; }

                }
                catch (Exception ex) { TempData["Errormessage"] = "Проблемы с чтением файла"; }

            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> ShowData(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Файл не выбран или пуст.";
                return RedirectToAction("Index");
            }

            try
            {
                var clients = await _ClientRepository.GetDataByXLSXAsync(file);
                HttpContext.Session.SetString("ClientsToUpload", JsonConvert.SerializeObject(clients));
                TempData["PreviewMode"] = "true";
                return View("Index", clients);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка при чтении файла: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> Delete()
        {
            _ClientRepository.ResetClientIdentity();
            int countRow = await _ClientRepository.GetClientsCountAsync();
            if (countRow >= 1)
            {
                TempData["Notification"] = $"Все данные с БД удалены. Количество удалённых данных: {countRow}";
                await _ClientRepository.Delete();
                return RedirectToAction("Index");
            }
            TempData["Notification"] = $"В БД нет данных";
            return RedirectToAction("Index");
        }

        public async Task <IActionResult> DeleteById(int Id)
        {
            _ClientRepository.ResetClientIdentity();
            await _ClientRepository.DeleteByID(Id);
            TempData["SuccessMessage"] = $"Клиент удалён";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SearchByPhone(string PhoneMobile)
        {
            if (string.IsNullOrEmpty(PhoneMobile))
            {
                TempData["ErrorMessage"] = "Введите номер телефона";
                return RedirectToAction("Index");
            }
            var clients = await _ClientRepository.GetByPhoneAsync(PhoneMobile);

            if (clients.Count == 0) { TempData["ErrorMessage"] = "Пользователь с таким номером не найден"; RedirectToAction("Index"); }

            return View("index", clients);

        }




    }
}
