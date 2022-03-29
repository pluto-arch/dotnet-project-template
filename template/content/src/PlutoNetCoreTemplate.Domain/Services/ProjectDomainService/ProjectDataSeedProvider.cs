using PlutoNetCoreTemplate.Domain.SeedWork;

namespace PlutoNetCoreTemplate.Domain.Services.ProjectDomainService
{
    using PlutoNetCoreTemplate.Domain.Aggregates.ProjectAggregate;
    using PlutoNetCoreTemplate.Domain.Repositories;

    using System;
    using System.Threading.Tasks;

    public class ProjectDataSeedProvider : IDataSeedProvider
    {
        private readonly IRepository<Project, int> _projectRepository;

        public ProjectDataSeedProvider(IRepository<Project, int> projectRepository)
        {
            _projectRepository = projectRepository;
        }


        public int Sorts => 100;

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            if (await _projectRepository.GetCountAsync() <= 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    var project = new Project { Name = $"Project{i}" };
                    await _projectRepository.InsertAsync(project);
                }

                await _projectRepository.Uow.SaveChangesAsync();
            }
        }
    }
}