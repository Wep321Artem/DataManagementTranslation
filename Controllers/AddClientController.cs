using DataManagementTranslation.Models;
using DataManagementTranslation.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataManagementTranslation.Controllers
{
    public class AddClientController : Controller
    {

        public IActionResult AddClient()
        {
            return View();
        }

        private readonly ClientRepository _ClientRepository;

        public AddClientController(ClientRepository clientRepository)
        {
            _ClientRepository = clientRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddOneClient(Clients model)
        {
            if (model.CardCode != null && model.PhoneMobile!=null)
            {
                if (ModelState.IsValid)
                {
                    await _ClientRepository.Add(
                        model.CardCode,
                        model.LastName,
                        model.SurName,
                        model.FirstName,
                        model.PhoneMobile,
                        model.Email,
                        model.GenderId,
                        model.Birthday,
                        model.City,
                        model.Pincode,
                        model.Bonus,
                        model.Turnover
                    );
                }
                return RedirectToAction("AddClient");

            }

            TempData["ErrorMessage"] = "Поля CardCode и PhoneMobile должны быть заполнены";

            return RedirectToAction("AddClient");



        }


    }
}
