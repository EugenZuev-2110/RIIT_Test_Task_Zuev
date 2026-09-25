namespace RIIT_Test_Task_Zuev.Core.Dtos;

/// <summary>
/// DTO для передачи элементов словаря (типов техники) на клиент для компонента Lookup.
/// </summary>
public class EquipmentTypeDto
{
    /// <summary>
    /// Идентификатор типа техники.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование типа техники.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}