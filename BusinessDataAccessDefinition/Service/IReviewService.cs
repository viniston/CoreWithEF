using System.Collections.Generic;
using BusinessEntitties;

namespace BusinessDataAccessDefinition.Service
{
    public interface IReviewService
    {
        List<Review> GetAll();
        Review Get(long id);
        Review Get(string name);
        Review Create(Review scenario);
        Review Update(Review item);
        bool Delete(long id);
        List<Review> GetSummary();
    }
}
