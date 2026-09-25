using Microsoft.EntityFrameworkCore;
using RIIT_Test_Task_Zuev.Core.Entities;

namespace RIIT_Test_Task_Zuev.Infrastructure.Data
{
    /// <summary>
    /// Класс для автоматического заполнения справочников базы данных при старте приложения.
    /// </summary>
    public static class AppDbContextSeed
    {
        /// <summary>
        /// Проверяет справочник типов техники и, если он пуст, наполняет его дефолтными значениями.
        /// </summary>
        /// <param name="context">Контекст базы данных Entity Framework.</param>
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!await context.EquipmentTypes.AnyAsync())
            {
                var defaultTypes = new[]
                {
                    new EquipmentType { Name = "Системный блок" },
                    new EquipmentType { Name = "Ноутбук" },
                    new EquipmentType { Name = "Монитор" },
                    new EquipmentType { Name = "МФУ / Принтер" }
                };

                await context.EquipmentTypes.AddRangeAsync(defaultTypes);
                await context.SaveChangesAsync();
            }
        }
    }
}
