using WorkFlowHub.Application.Interfaces;
using WorkFlowHub.Domain.Entities;
using WorkFlowHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace WorkFlowHub.Infrastructure.Repositories
{
    public class DepartmentRepository: IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
        }

        public void Update(Department department)
        {
            _context.Departments.Update(department);
        }

        public void Delete(Department department)
        {
            _context.Departments.Remove(department);
        }

        public async Task<bool> ExistsByNameAsync(string name,int? excludeId = null)
        {
            return await _context.Departments
                .AnyAsync(d => d.Name == name &&
                      (!excludeId.HasValue || d.Id != excludeId.Value));
                
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

