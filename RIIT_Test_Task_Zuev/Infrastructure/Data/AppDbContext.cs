using Microsoft.EntityFrameworkCore;
using RIIT_Test_Task_Zuev.Core.Entities;

namespace RIIT_Test_Task_Zuev.Infrastructure.Data;

/// <summary>
/// Контекст базы данных Entity Framework Core для управления сущностями компьютерной техники.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AppDbContext"/> с заданными параметрами.
    /// </summary>
    /// <param name="options">Параметры конфигурации контекста (строка подключения, провайдер БД).</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Таблица компьютерной техники.
    /// </summary>
    public DbSet<Equipment> Equipments => Set<Equipment>();

    /// <summary>
    /// Таблица справочника типов техники.
    /// </summary>
    public DbSet<EquipmentType> EquipmentTypes => Set<EquipmentType>();

    /// <summary>
    /// Конфигурация маппинга моделей на таблицы PostgreSQL с использованием Fluent API.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка сущности EquipmentType (Словарь типов техники)
        modelBuilder.Entity<EquipmentType>(entity =>
        {
            entity.ToTable("equipment_types");

            entity.HasKey(e => e.Id);

            // Ограничение по ТЗ: Наименование до 128 символов
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);

            // Уникальный индекс для наименования типа, чтобы избежать дубликатов в словаре
            entity.HasIndex(e => e.Name)
                .IsUnique();
        });

        // Настройка сущности Equipment (Компьютерная техника)
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.ToTable("equipment");

            entity.HasKey(e => e.Id);

            // Ограничение по ТЗ: Учетный номер до 32 символов
            entity.Property(e => e.InventoryNumber)
                .IsRequired()
                .HasMaxLength(32);

            // Ограничение по ТЗ: Учетный номер должен быть уникальным
            entity.HasIndex(e => e.InventoryNumber)
                .IsUnique();

            // Ограничение по ТЗ: Наименование до 256 символов
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);

            // Ограничение по ТЗ: Комната размещения от 1 до 1000.
            // Добавляем Check Constraint на уровне базы данных PostgreSQL для надежности
            entity.Property(e => e.RoomNumber)
                .IsRequired();

            entity.ToTable(t => t.HasCheckConstraint("CK_Equipment_RoomNumber", "room_number >= 1 AND room_number  e.EquipmentType)
                .WithMany(t => t.Equipments)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict); // Запрещаем удаление типа, если на него ссылается техника
        });
    }
}