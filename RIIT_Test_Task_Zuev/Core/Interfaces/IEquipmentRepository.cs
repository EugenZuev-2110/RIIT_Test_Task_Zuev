using RIIT_Test_Task_Zuev.Core.Entities;

namespace RIIT_Test_Task_Zuev.Core.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для управления компьютерной техникой и справочниками.
    /// </summary>
    public interface IEquipmentRepository
    {
        /// <summary>
        /// Возвращает полный список компьютерной техники с включенными типами оборудования.
        /// </summary>
        Task<IEnumerable<Equipment>> GetAllAsync();

        /// <summary>
        /// Возвращает список всех доступных типов техники из словаря.
        /// </summary>
        Task<IEnumerable<EquipmentType>> GetTypesAsync();

        /// <summary>
        /// Добавляет новую единицу техники с проверкой ограничений.
        /// </summary>
        /// <param name="equipment">Сущность техники для добавления.</param>
        /// <returns>Результат операции: true — успешно, false — лимит базы данных превышен.</returns>
        Task<bool> AddAsync(Equipment equipment);

        /// <summary>
        /// Обновляет данные существующей единицы техники.
        /// </summary>
        Task UpdateAsync(Equipment equipment);

        /// <summary>
        /// Проверяет, существует ли уже техника с таким учетным номером.
        /// </summary>
        Task<bool> ExistsByInventoryNumberAsync(string inventoryNumber, int? excludeId = null);
    }
}
