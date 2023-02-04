using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DripChipDbSystem.Database.Models.Auth
{
    /// <summary>
    /// Аккаунт пользователя
    /// </summary>
    public class Account : IEntityTypeConfiguration<Account>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string PasswordHash { get; set; }

        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("account");
            builder.ToTable(x => x.HasCheckConstraint($"CK_{nameof(FirstName)}", $"[{nameof(FirstName)}] NOT NULL]"));
            builder.ToTable(x => x.HasCheckConstraint($"CK_{nameof(LastName)}", $"[{nameof(LastName)}] NOT NULL]"));
            builder.ToTable(x => x.HasCheckConstraint($"CK_{nameof(Email)}", $"[{nameof(Email)}] NOT NULL]"));
            builder.ToTable(x => x.HasCheckConstraint($"CK_{nameof(PasswordHash)}", $"[{nameof(PasswordHash)}] NOT NULL]"));
            builder.HasKey(x => x.Id);
        }
    }
}
