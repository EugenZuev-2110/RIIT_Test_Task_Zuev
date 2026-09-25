namespace RIIT_Test_Task_Zuev.Core.Entities;

/// <summary>
/// Сущность справочника типов компьютерной техники (например: Монитор, Ноутбук).
/// </summary>
public class EquipmentType
{
    /// <summary>
    /// Уникальный идентификатор типа техники.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование типа техники. Ограничение: до 128 символов.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Навигационное свойство. Список техники, относящейся к данному типу.
    /// </summary>
    public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
}