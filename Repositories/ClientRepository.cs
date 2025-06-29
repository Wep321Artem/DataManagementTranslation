using System.Linq;
using System.Threading.Tasks;
using DataManagementTranslation.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace DataManagementTranslation.Repositories
{
    public class ClientRepository
    {
        private readonly DataManagerDbContext _dbContext;

        public ClientRepository(DataManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Clients>> Get()
        {
            return await _dbContext.Client
                .AsNoTracking()
                .OrderBy(c => c.CardCode)
                .ToListAsync();
        }

        public async Task<List<Clients>> GetByFirstName(string FirstName)
        {
            var query = _dbContext.Client.AsNoTracking();

            if (string.IsNullOrEmpty(FirstName))
            {
                query = query.Where(n => n.FirstName == FirstName);
            }

            return await query.ToListAsync();

        }
        
        public async Task Add(int? CardCode, string? LastName, string? SurName, string? FirstName, string? PhoneMobile, string? Email, string? GenderId, DateTime? Birthday, string? City, int? Pincode, int? Bonus, int? Turnover)
        {
           
            
            var ClientsEntry = new Clients()
            {
                CardCode = CardCode,
                FirstName = FirstName,
                SurName = SurName,
                PhoneMobile = PhoneMobile,
                Email = Email,
                GenderId = GenderId,
                Birthday = Birthday,
                City = City,
                Pincode = Pincode,
                Bonus = Bonus,
                Turnover = Turnover,
                LastName = LastName
            };

                await _dbContext.AddAsync(ClientsEntry);
                await _dbContext.SaveChangesAsync();
        }

        public void ResetClientIdentity()
        {
            // Сбросить счетчик Identity для таблицы Client на 0
            _dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Client', RESEED, 0)");
        }

        public async Task Delete()
        {
            await _dbContext.Client.ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByID(int id)
        {
            await _dbContext.Client.Where(c => c.Id == id).ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, int? CardCode, string? LastName, string? SurName, string? FirstName, string? PhoneMobile, string? Email, string? GenderId, DateTime? Birthday, string? City, int? Pincode, int? Bonus, int? Turnover)
        {
            await _dbContext.Client
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.CardCode, CardCode)
                .SetProperty(c => c.FirstName, FirstName)
                .SetProperty(c => c.SurName, SurName)
                .SetProperty(c => c.LastName, LastName)
                .SetProperty(c => c.PhoneMobile, PhoneMobile)
                .SetProperty(c => c.Email, Email)
                .SetProperty(c => c.GenderId, GenderId)
                .SetProperty(c => c.Birthday, Birthday)
                .SetProperty(c => c.City, City)
                .SetProperty(c => c.Pincode, Pincode)
                .SetProperty(c => c.Bonus, Bonus)
                .SetProperty(c => c.Turnover, Turnover));

            await _dbContext.SaveChangesAsync();

        }


        public async Task<List<Clients>> GetDataByXLSX (IFormFile file) // IFormFile - это интерфейс, представляющий файл отправленный через HTTP-запрос (обычно через форму). 
        {
            List<Clients> clients = new List<Clients>();

            //установим лицензию для бибилиотеки EPPlus

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //Специально устанавливаем 7.9 версию библиотеки, чтобы можно было прописать использовать некомерческую лицензию

            using (var stream = new MemoryStream()) // используем MemoryStream для работы с файлом. То есть мы создаём поток данных, который хранится в оперативной памяти пк. Можно файл не сохранять на диск
            {
                await file.CopyToAsync(stream); // копируем туда файл

                using (var package = new ExcelPackage(stream)) //Основной класс EPPlus, который представляет Excel-файл в памяти.
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Получаем первый лист Excel

                    if(worksheet.Dimension == null) { throw new InvalidOperationException("Файл пустой или не содержит данных"); }
                    
                    int rowCount = worksheet.Dimension.Rows;
                    
                    if(rowCount <= 1) { throw new InvalidOperationException("В файле нет данных, только заголовки"); }

                    //Перебор данных
                    for(int row = 2; row<= rowCount; row++) // начинаем со второй строки т.к первая строка - загоовки
                    {

                        var client = new Clients
                        {
                            CardCode = int.TryParse(worksheet.Cells[row, 1].Text?.Trim(), out int code) ? code : (int?)null,
                            LastName = worksheet.Cells[row,2].Text?.Trim(), 
                            FirstName = worksheet.Cells[row,3].Text?.Trim(),
                            SurName = worksheet.Cells[row,4].Text?.Trim(),
                            PhoneMobile = worksheet.Cells[row, 5].Text?.Trim(),
                            Email = worksheet.Cells[row, 6].Text?.Trim(),
                            GenderId = worksheet.Cells[row, 7].Text?.Trim(),
                            Birthday = DateTime.TryParse(worksheet.Cells[row, 8].Text?.Trim(), out var birthday)? birthday: (DateTime?)null,
                            City = worksheet.Cells[row, 9].Text?.Trim(),
                            Pincode = int.TryParse(worksheet.Cells[row, 10].Text?.Trim(), out int pincode) ? pincode : (int?)null,
                            Bonus = int.TryParse(worksheet.Cells[row, 11].Text?.Trim(), out int bonus) ? bonus : (int?)null,
                            Turnover = int.TryParse(worksheet.Cells[row, 12].Text?.Trim(), out int turnover) ? turnover : (int?)null
                        };
                        if (client.CardCode == null) { continue;}
                        clients.Add(client);
                    }
                }

            }
            return clients;

        }

        public Clients GetClientByID(int id)
        {
            return _dbContext.Client.FirstOrDefault(c => c.Id==id);

        }
        public async Task<int> GetClientsCountAsync()
        {
            return await _dbContext.Client.CountAsync();
        }

        public async Task<List<Clients>> GetByPhone(string phone)
        {
            return await _dbContext.Client.Where(p => p.PhoneMobile == phone).ToListAsync();


        }





    }
}
