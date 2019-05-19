using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BusinessDataAccessDefinition.Models;

namespace BusinessDataAccess.EF
{
    public class InitialData
    {
        private readonly ReviewDbContext _dbContext;

        public InitialData(ReviewDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Making sure that database has nothing before seeding
        public void SeedData()
        {
            if (_dbContext.Review.Any()) return;
            var review = new Review { Id = 0, Name = $"Test Review-{DateTime.Now.Ticks}" };
            _dbContext.Review.Add(review);
            _dbContext.SaveChanges();
        }
    }
}
