namespace Startup.Configuration
{
    /// <summary>
    /// Конфигурации базы данных
    /// </summary>
    public class DatabaseConfig
    {
        /// <summary>
        /// Имя в конфиге
        /// </summary>
        public static string ConfigName => "Database";

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
