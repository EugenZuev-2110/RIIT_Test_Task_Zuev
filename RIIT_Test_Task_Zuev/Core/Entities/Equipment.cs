namespace RIIT_Test_Task_Zuev.Core.Entities;

/// <summary>
/// Сущность единицы компьютерной техники.
/// </summary>
public class Equipment
{
    /// <summary>
    /// Уникальный идентификатор единицы техники.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Учетный номер. Должен быть уникальным и состоять только из цифр и латинских букв. 
    /// Ограничение: до 32 символов.
    /// </summary>
    public string InventoryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Произвольное наименование техники. Ограничение: до 256 символов.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор типа техники (внешний ключ).
    /// </summary>
    public int TypeId { get; set; }

    /// <summary>
    /// Навигационное свойство для связи со справочником типов техники.
    /// </summary>
    public virtual EquipmentType EquipmentType { get; set; } = null!;

    /// <summary>
    /// Номер комнаты размещения оборудования. Ограничение: от 1 до 1000.
    /// </summary>
    public int RoomNumber { get; set; }
}