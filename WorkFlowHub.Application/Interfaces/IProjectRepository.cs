using WorkFlowHub.Domain.Entities;
namespace WorkFlowHub.Application.Interfaces
{
    public interface IProjectRepository
    {
        IQueryable<Project> Query();
        Task<Project?> GetByIdAsync(int id);
        Task AddAsync(Project project);
        void Update(Project project);
        void Delete(Project project);
        Task SaveChangesAsync();

    }
}
