using System;
using System.Collections.Generic;
using System.Text;
using WorkFlowHub.Application.DTOs.Departments;
using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Application.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync();
        Task<List<DepartmentDto>>GetAllWithEmployeeCountAsync();

        Task<Department?> GetByIdAsync(int id);

        Task AddAsync(Department department);

        void Update(Department department);

        void Delete(Department department);

        Task<bool> ExistsByNameAsync(string name,int? excludeId=null);
        Task<bool> ExistAsync(int id);
        Task SaveChangesAsync();
    }
}
