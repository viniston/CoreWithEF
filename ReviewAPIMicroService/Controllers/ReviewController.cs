using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using BusinessDataAccessDefinition.Service;
using BusinessEntitties;
using Microsoft.AspNetCore.Mvc;

namespace ReviewAPIMicroService.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewController(IReviewService reiewService, IMapper mapper)
        {
            _reviewService = reiewService;
            _mapper = mapper;
        }

        //GET: api/Review
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<ViewModels.ReviewSummary>> Get()
        {
            var resultDomainModel = _reviewService.GetSummary();

            if (resultDomainModel == null)
                return NotFound("Reviews not found");

            // Project to mapped ViewModel object
            var result = _mapper.Map<IEnumerable<Review>, List<ViewModels.ReviewSummary>>(resultDomainModel);

            if (result != null)
                return Ok(result.OrderBy(x => x.Name).ToList());

            return NotFound("Reviews not found");
        }


        // GET: api/Review/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ViewModels.Review> Get(long id)
        {
            var resultDomainModel = _reviewService.Get(id);

            // Project to mapped ViewModel object
            var result = _mapper.Map<Review, ViewModels.Review>(resultDomainModel);

            if (result != null)
                return Ok(result);

            return NotFound($"Review not found with id {id}");
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<long> Post(ViewModels.Review review)
        {
            var request = _mapper.Map<ViewModels.Review, Review>(review);
            var result = _reviewService.Create(request);
            return Ok(result.Id);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult Put(ViewModels.Review review)
        {
            var request = _mapper.Map<ViewModels.Review, Review>(review);
            _reviewService.Update(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult Delete([Required]long id)
        {
            _reviewService.Delete(id);
            return Ok();
        }
    }
}