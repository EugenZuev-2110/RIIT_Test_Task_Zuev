using Microsoft.EntityFrameworkCore;
using RIIT_Test_Task_Zuev.Core.Entities;
using RIIT_Test_Task_Zuev.Core.Interfaces;
using RIIT_Test_Task_Zuev.Infrastructure.Data;

namespace RIIT_Test_Task_Zuev.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с PostgreSQL через Entity Framework Core.
    /// </summary>
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AppDbContext _context;
        private const int MaxRecordsLimit = 2000;

        public EquipmentRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Equipment>> GetAllAsync()
        {
            return await _context.Equipments
                .Include(e => e.EquipmentType)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<EquipmentType>> GetTypesAsync()
        {
            return await _context.EquipmentTypes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> AddAsync(Equipment equipment)
        {
            var currentCount = await _context.Equipments.CountAsync();
            if (currentCount >= MaxRecordsLimit)
            {
                return false;
            }

            await _context.Equipments.AddAsync(equipment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(Equipment equipment)
        {
            _context.Entry(equipment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByInventoryNumberAsync(string inventoryNumber, int? excludeId = null)
        {
            return await _context.Equipments
                .AnyAsync(e => e.InventoryNumber == inventoryNumber && (!excludeId.HasValue || e.Id != excludeId.Value));
        }
    }
}
