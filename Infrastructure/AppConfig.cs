namespace DataManagementTranslation.Infrastructure
{
    public class AppConfig //работа  с jsone файлами
    {

        public Database Database { get; set; } = new Database();
        public InfoProject InfoProject { get; set; } = new InfoProject();
    }

    public class Database
    {
        public string? ConnectionString { get; set; }

    }

    public class InfoProject
    { 
        public string? ProjectName { get; set; }

    }
}
