using System.ComponentModel.DataAnnotations;

namespace RIIT_Test_Task_Zuev.Core.Dtos;

/// <summary>
/// DTO для создания, редактирования и отображения компьютерной техники в UI DevExtreme.
/// </summary>
public class EquipmentDto
{
    /// <summary>
    /// Идентификатор записи. Равен 0 при создании новой записи.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Учетный номер техники. Обязателен, до 32 символов, только латиница и цифры.
    /// </summary>
    [Required(ErrorMessage = "Учетный номер обязателен для заполнения")]
    [StringLength(32, ErrorMessage = "Учетный номер не должен превышать 32 символа")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Учетный номер должен содержать только латинские буквы и цифры")]
    public string InventoryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Наименование техники. Обязательно, до 256 символов.
    /// </summary>
    [Required(ErrorMessage = "Наименование обязательно для заполнения")]
    [StringLength(256, ErrorMessage = "Наименование не должно превышать 256 символов")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор выбранного типа техники из связанного словаря.
    /// </summary>
    [Required(ErrorMessage = "Тип техники обязателен")]
    public int TypeId { get; set; }

    /// <summary>
    /// Текстовое наименование типа техники (используется для плоского вывода в таблице DevExtreme).
    /// </summary>
    public string? TypeName { get; set; }

    /// <summary>
    /// Номер комнаты. Обязателен, строго в диапазоне от 1 до 1000.
    /// </summary>
    [Required(ErrorMessage = "Номер комнаты обязателен")]
    [Range(1, 1000, ErrorMessage = "Комната размещения должна быть в диапазоне от 1 до 1000")]
    public int RoomNumber { get; set; }
}