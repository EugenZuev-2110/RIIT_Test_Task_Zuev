using Microsoft.AspNetCore.Mvc;
using RIIT_Test_Task_Zuev.Core.Dtos;
using RIIT_Test_Task_Zuev.Core.Entities;
using RIIT_Test_Task_Zuev.Core.Interfaces;

namespace RIIT_Test_Task_Zuev.WebApi.Controllers;

/// <summary>
/// API-контроллер для управления компьютерной техникой.
/// Предоставляет REST эндпоинты для тонкого клиента DevExtreme.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentRepository _repository;

    public EquipmentController(IEquipmentRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Получает весь список компьютерной техники.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var equipmentList = await _repository.GetAllAsync();

            var dtos = equipmentList.Select(e => new EquipmentDto
            {
                Id = e.Id,
                InventoryNumber = e.InventoryNumber,
                Name = e.Name,
                TypeId = e.TypeId,
                TypeName = e.EquipmentType?.Name ?? "Не указан",
                RoomNumber = e.RoomNumber
            });

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера при получении данных: {ex.Message}");
        }
    }

    /// <summary>
    /// Получает список типов техники для заполнения выпадающего списка (Lookup) в таблице.
    /// </summary>
    [HttpGet("types")]
    public async Task<IActionResult> GetTypes()
    {
        var types = await _repository.GetTypesAsync();
        var dtos = types.Select(t => new EquipmentTypeDto
        {
            Id = t.Id,
            Name = t.Name
        });

        return Ok(dtos);
    }

    /// <summary>
    /// Создает новую единицу техники с проверкой лимита на 2000 записей и уникальности номера.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EquipmentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _repository.ExistsByInventoryNumberAsync(dto.InventoryNumber))
        {
            return BadRequest("Техника с таким учетным номером уже зарегистрирована в системе.");
        }

        var equipment = new Equipment
        {
            InventoryNumber = dto.InventoryNumber,
            Name = dto.Name,
            TypeId = dto.TypeId,
            RoomNumber = dto.RoomNumber
        };

        var success = await _repository.AddAsync(equipment);
        if (!success)
        {
            return BadRequest("Невозможно добавить запись. Достигнут жесткий лимит базы данных (максимум 2000 записей).");
        }

        // Возвращаем созданный объект с его новым ID
        dto.Id = equipment.Id;
        return CreatedAtAction(nameof(GetAll), new { id = equipment.Id }, dto);
    }

    /// <summary>
    /// Редактирует существующую единицу техники.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] System.Text.Json.JsonElement json)
    {
        try
        {
            var allEquipment = await _repository.GetAllAsync();
            var equipment = allEquipment.FirstOrDefault(e => e.Id == id);

            if (equipment == null)
            {
                return NotFound("Запись не найдена.");
            }

            if (json.TryGetProperty("inventoryNumber", out var invNumProp))
            {
                var newInvNum = invNumProp.GetString() ?? string.Empty;

                if (await _repository.ExistsByInventoryNumberAsync(newInvNum, id))
                {
                    return BadRequest("Указанный учетный номер уже используется другим оборудованием.");
                }
                equipment.InventoryNumber = newInvNum;
            }

            if (json.TryGetProperty("name", out var nameProp))
            {
                equipment.Name = nameProp.GetString() ?? string.Empty;
            }

            if (json.TryGetProperty("typeId", out var typeProp))
            {
                equipment.TypeId = typeProp.GetInt32();
            }

            if (json.TryGetProperty("roomNumber", out var roomProp))
            {
                equipment.RoomNumber = roomProp.GetInt32();
            }

            await _repository.UpdateAsync(equipment);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка при обновлении записи: {ex.Message}");
        }
    }

}