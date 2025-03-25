using Contracts;
using Entities.Models;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company, Guid>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        return FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();
    }

    public Company? GetCompany(Guid companyId, bool trackChanges)
    {
        return FindByCondition(c => c.Id.Equals(companyId), trackChanges)
            .SingleOrDefault();
    }
}