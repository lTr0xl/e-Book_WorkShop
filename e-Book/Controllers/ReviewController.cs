using e_Book.Interfaces;
using e_Book.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_Book.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public IActionResult CreateReview(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to create review");
                return View(review);
            }
            var newReview = new Review
            {
                BookId = id,
                AppUser = review.AppUser,
                Comment = review.Comment,
                Rating = review.Rating,
            };
            _reviewRepository.Add(newReview);
            return Redirect("/Book/Details/" + id);
        }
        public async Task<IActionResult> DeleteReview(int id)
        {
            Review review = await _reviewRepository.GetByIdAsync(id);
            if (review != null)
            {
                return View(review);
            }
            return View("Error");
        }
        [HttpPost, ActionName("DeleteReview")]
        public async Task<IActionResult> DeleteReviewPage(int id)
        {
            Review review = await _reviewRepository.GetByIdAsync(id);
            var bookId = review.BookId;
            if (review != null)
            {
                _reviewRepository.Delete(review);
                return Redirect("/Book/Details/" + bookId);
            }
            return View("Error");
        }
        public async Task<IActionResult> EditReview(int id)
        {
            Review review = await _reviewRepository.GetByIdAsync(id);
            if (review != null)
            {
                return View(review);
            }
            return View("Error");
        }
        [HttpPost]
        public async Task<IActionResult> EditReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("EditReview", review);
            }
            Review newReview = new Review()
            {
                Id = id,
                BookId = review.BookId,
                AppUser = review.AppUser,
                Comment = review.Comment,
                Rating = review.Rating,
            };
            _reviewRepository.Update(newReview);
            return Redirect("/Book/Details/" + newReview.BookId);
        }
    }
}
