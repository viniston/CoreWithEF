using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessDataAccess.EF;
using BusinessDataAccessDefinition.Service;
using BusinessEntitties;
using Microsoft.EntityFrameworkCore;
using DataModels = BusinessDataAccessDefinition.Models;

namespace BusinessDataAccess.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ReviewDbContext _context;
        private readonly IMapper _mapper;

        public ReviewService(ReviewDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Review> GetAll()
        {
            var reviews = _context.Review.ToList();
            var returnedReviews = _mapper.Map<List<Review>>(reviews);
            return returnedReviews;
        }

        public Review Get(long id)
        {
            var review = _context.Review.AsTracking().FirstOrDefault(r => r.Id == id);
            var returnedReview = _mapper.Map<Review>(review);
            return returnedReview;
        }

        public Review Get(string name)
        {
            var review = _context.Review.AsNoTracking().FirstOrDefault(r => r.Name == name);
            var returnedReview = _mapper.Map<Review>(review);
            return returnedReview;
        }

        public Review Create(Review scenario)
        {
            var entity = _mapper.Map<DataModels.Review>(scenario);
            _context.Review.Add(entity);
            _context.SaveChanges();
            return _mapper.Map<Review>(entity);
        }

        public Review Update(Review item)
        {
            var originalItem = Get(item.Id);
            var originalItemMapped = _mapper.Map<DataModels.Review>(originalItem);

            var mappedItem = _mapper.Map<DataModels.Review>(item);

            // TODO: rather we can directly update mappedItem
            originalItemMapped.Name = mappedItem.Name;
            originalItemMapped.Description = mappedItem.Description;
            originalItemMapped.IsActive = mappedItem.IsActive;

            _context.Review.Update(originalItemMapped);
            _context.SaveChanges();
            _context.DetachAllEntities();

            return _mapper.Map<Review>(originalItemMapped);
        }

        public bool Delete(long id)
        {
            var item = _context.Review.Find(id);
            var result = _context.Review.Remove(item);
            _context.SaveChanges();
            return result.State == EntityState.Unchanged;
        }

        public List<Review> GetSummary()
        {
            var reviews = _context.Review
                .AsNoTracking().ToList();

            var returnedReviews = _mapper.Map<List<Review>>(reviews);

            return returnedReviews;
        }
    }
}
