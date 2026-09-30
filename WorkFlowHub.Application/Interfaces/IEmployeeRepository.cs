using WorkFlowHub.Domain.Entities;

namespace WorkFlowHub.Application.Interfaces;

public interface IEmployeeRepository
{
    IQueryable<Employee> Query();

    Task<Employee?> GetByIdAsync(int id);

    Task AddAsync(Employee employee);

    void Update(Employee employee);

    void Delete(Employee employee);
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);

    Task SaveChangesAsync();
}