using AutoMapper;
using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ICompanyInterFaceRepositry _companyRepository;
    private readonly interfaceRepastory _userRepository;
    public ReviewController(IReviewRepository reviewRepository, ICompanyInterFaceRepositry companyInterFaceRepositry, interfaceRepastory interfaceRepastory, IConfiguration configuration)
    {
        _reviewRepository = reviewRepository;
        _companyRepository = companyInterFaceRepositry;
        _userRepository = interfaceRepastory;
    }

    [HttpGet("GetReview")]
    public async Task<ActionResult<List<ReviewBrief>>> GetReviews()
    {
        var reviews = await _reviewRepository.GetAllReviewsAsync();
        if (reviews == null || !reviews.Any())
        {
            return NotFound("No reviews found.");
        }
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewDto>> GetReview(int id)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        var reviewDto = new ReviewDto
        {
            ReviewId = review.ReviewId,
            Name = review.Name,
            Content = review.Content,
            RegistrationId = review.RegistrationId,
        };

        return Ok(reviewDto);
    }
    [HttpPost("addReview")]
    public async Task<IActionResult> AddReview([FromBody] CreateReviewDto reviewDto)
    {

        var company = _companyRepository.GetCompany((int)reviewDto.RegistrationId);
        var user = _userRepository.GetSingleUserAsync(reviewDto.UserId); // Fetch User based on ID

        if (company == null || user == null)
        {
            return BadRequest("Company or User not found.");
        }

        var reviewEntity = new Review
        {
            Name = reviewDto.Name,
            Content = reviewDto.Content,
            RegistrationId = reviewDto.RegistrationId,
            Company = company, // Set Company
            User = user // Set User
        };

        await _reviewRepository.AddReviewAsync(reviewEntity);
        if (_reviewRepository.SaveChanges())
        {
            return Ok(new { message = "Review added successfully" });
        }

        return BadRequest("Failed to add review");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto updateReviewDto)
    {
        var existingReview = await _reviewRepository.GetReviewByIdAsync(id);
        if (existingReview == null)
        {
            return NotFound();
        }

        existingReview.Name = updateReviewDto.Name;
        existingReview.Content = updateReviewDto.Content;
        existingReview.RegistrationId = updateReviewDto.RegistrationId;
        await _reviewRepository.UpdateReviewAsync(existingReview);
        return NoContent();
    }

    [HttpPut("EditReviewByName/{name}")]
    public IActionResult EditReview(string name, [FromBody] Review review)
    {
        var reviewDb = _reviewRepository.GetReviewByName(name);
        if (reviewDb != null)
        {
            reviewDb.Name = review.Name;
            reviewDb.Content = review.Content;

            if (_reviewRepository.SaveChanges())
            {
                return Ok();
            }
            return BadRequest("Failed to edit review");
        }
        return NotFound("Review not found");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteReview(int id)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        await _reviewRepository.DeleteReviewAsync(id);
        return NoContent();
    }

    [HttpDelete("DeleteReviewByNameAndRegistrationId/{registrationId}")]
    public async Task<IActionResult> DeleteReviewByName(string name, int registrationId)
    {
        try
        {
            await _reviewRepository.DeleteReviewByNameAndRegistrationId(name, registrationId);
            return Ok("Review deleted successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
