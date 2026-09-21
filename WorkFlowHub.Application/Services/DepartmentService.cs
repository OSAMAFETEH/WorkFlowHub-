using System;
using System.Collections.Generic;
using System.Text;
using WorkFlowHub.Application.Interfaces;
using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Application.Services
{
    public class DepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<(bool Success, string? Error)> CreateAsync(
            string name,
            string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return (false, "Department name is required.");
            }

            name = name.Trim();

            if (await _repository.ExistsByNameAsync(name))
            {
                return (false, "Department already exists.");
            }

            var department = new Department
            {
                Name = name,
                Description = description?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(department);
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
