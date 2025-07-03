using System.Threading.Tasks;
using DataManagementTranslation.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DataManagementTranslation.Controllers
{
    public class EditingClientController : Controller
    {
        private readonly ClientRepository _ClientRepository;

        public EditingClientController(ClientRepository ClientRepository)
        {
            _ClientRepository = ClientRepository;
        }
        [HttpGet]
        public async Task<IActionResult> EditingClient(int id)
        {
            var client = await _ClientRepository.GetClientByIDAsync(id);
            if (client == null) { return NotFound(); }
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEditingClient(int id, int? CardCode, string? LastName, string? SurName, string? FirstName, string? PhoneMobile, string? Email, string? GenderId, DateTime? Birthday, string? City, int? Pincode, int? Bonus, int? Turnover)
        {
            if (CardCode != null && PhoneMobile!=null)
            {
                await _ClientRepository.Update(id, CardCode, LastName, SurName, FirstName, PhoneMobile, Email, GenderId, Birthday, City, Pincode, Bonus, Turnover);
                return RedirectToAction("Index", "Home");

            }
            TempData["ErrorMessage"] = "Поля CardCode и PhoneMobile должны быть заполнены";
            return RedirectToAction("EditingClient", new { id = id });


        }


    }
}
