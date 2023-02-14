namespace Startup.Configuration
{
    /// <summary>
    /// Конфигурации базы данных
    /// </summary>
    public class DatabaseConfig
    {
        public static string ConfigName => "Database";

        /// <summary>
        /// Хост расположения бд
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Сервер расположения бд
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// Порт
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Имя бд
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
