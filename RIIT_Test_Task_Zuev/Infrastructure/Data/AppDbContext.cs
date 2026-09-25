using Microsoft.EntityFrameworkCore;
using RIIT_Test_Task_Zuev.Core.Entities;

namespace RIIT_Test_Task_Zuev.Infrastructure.Data
{
    /// <summary>
    /// Контекст базы данных Entity Framework Core для управления сущностями компьютерной техники.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AppDbContext"/>
        /// </summary>
        /// <param name="options">Параметры конфигурации контекста.</param>
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
        /// Конфигурация маппинга моделей.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EquipmentType>(entity =>
            {
                entity.ToTable("equipment_types");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("equipment");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.InventoryNumber)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.HasIndex(e => e.InventoryNumber)
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.RoomNumber)
                    .IsRequired();

                entity.HasOne(e => e.EquipmentType)
                    .WithMany(t => t.Equipments)
                    .HasForeignKey(e => e.TypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}