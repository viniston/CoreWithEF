using System;
using BusinessDataAccessDefinition.Models;

namespace BusinessDataAccess.EF
{
    public static class DbInitializer
    {

        public static void Initialize(ReviewDbContext context)
        {
            context.Database.EnsureCreated();
            var skill = new Review { Id = 0, Name = $"Test Review-{DateTime.Now.Ticks}" };
            context.Review.Add(skill);
        }
    }
}
