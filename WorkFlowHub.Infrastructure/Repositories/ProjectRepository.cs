using WorkFlowHub.Application.Interfaces;
using WorkFlowHub.Infrastructure.Data;
using WorkFlowHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace WorkFlowHub.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Project> Query()
        {
            return _context.Projects.AsNoTracking();
        }
        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }
        public void Update(Project project)
        {
            _context.Projects.Update(project);
        }
        public void Delete(Project project)
        {
            _context.Projects.Remove(project);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
