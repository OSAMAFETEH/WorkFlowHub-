using System;
using System.Collections.Generic;
using System.Text;
using WorkFlowHub.Application.Interfaces;
using WorkFlowHub.Domain.Entities;
using WorkFlowHub.Application.DTOs.Departments;
namespace WorkFlowHub.Application.Services
{
    public class DepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DepartmentDto>> GetAllAsync()
        {
            var departments = await _repository.GetAllAsync();

            return departments
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt,
                    EmployeeCount = 0
                })
                .ToList();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<(bool Success, string? Error)> CreateAsync(CreateDepartmentsDto dto)
        {
            var name= dto.Name.Trim();

            if (await _repository.ExistsByNameAsync(name))
            {
                return (false, "Department already exists.");
            }

            Department department = new Department
            {
                Name = name,
                Description = dto.Description?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(department);
            await _repository.SaveChangesAsync();

            return (true, null);
        }
        public async Task<(bool Success, string? Error)> UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var department = await _repository.GetByIdAsync(id);
            if (department is null)
            {
                return (false, "Department not found.");
            }
            var name = dto.Name.Trim();

            if (await _repository.ExistsByNameAsync(name, id))
            {
                return (false, "Another department already uses this name.");
            }
            department.Name = name;
            department.Description = dto.Description?.Trim();

            _repository.Update(department);

            await _repository.SaveChangesAsync();

            return (true, null);

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department is null)
            {
                return false;
            }

            _repository.Delete(department);

            await _repository.SaveChangesAsync();

            return true;
        }
    }

}
