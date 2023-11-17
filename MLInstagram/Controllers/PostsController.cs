using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLInstagram.Data;
using MLInstagram.Models;
using Amazon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace MLInstagram.Controllers
{
	[Authorize]
	public class PostsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _config;
		private readonly IOptions<AWSUserInfo> _options;
		public PostsController(ApplicationDbContext context, IConfiguration config, IOptions<AWSUserInfo> options)
		{
			_context = context;
			_config = config;
			_options = options;
		}

		// GET: Posts
		public async Task<IActionResult> Index()
		{
			return _context.Posts != null ?
						View(await _context.Posts.ToListAsync()) :
						Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
		}

		// GET: Posts/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Posts == null)
			{
				return NotFound();
			}

			var posts = await _context.Posts
				.FirstOrDefaultAsync(m => m.Id == id);
			if (posts == null)
			{
				return NotFound();
			}

			return View(posts);
		}

		// GET: Posts/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Posts/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(IFormFile file)
		{
			//access key and secret key with S3 region
			using (var amazonS3client = new AmazonS3Client(_options.Value.AccessKey, _options.Value.SecretKey, RegionEndpoint.APSoutheast1))
			{
				using (var memorystream = new MemoryStream())
				{
					file.OpenReadStream().CopyTo(memorystream);
					var request = new TransferUtilityUploadRequest
					{
						InputStream = memorystream,
						Key = file.FileName,
						BucketName = _options.Value.S3Bucket,
						ContentType = file.ContentType,
						CannedACL = S3CannedACL.PublicRead
					};
					var transferUtility = new TransferUtility(amazonS3client);
					await transferUtility.UploadAsync(request);
				}
			}
			Post fileDetails = new Post();
			fileDetails.FileName = file.FileName;
			fileDetails.DatePosted = DateTime.Now;

			_context.Posts.Add(fileDetails);
			_context.SaveChanges();

			ViewBag.Success = "File Uploaded Successfully on S3 Bucket";
			return RedirectToAction(nameof(Index));
		}

		// GET: Posts/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Posts == null)
			{
				return NotFound();
			}

			var posts = await _context.Posts.FindAsync(id);
			if (posts == null)
			{
				return NotFound();
			}
			return View(posts);
		}

		// POST: Posts/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,DatePosted,Caption,Length,Width,Duration")] Post posts)
		{
			if (id != posts.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(posts);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PostsExists(posts.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(posts);
		}

		// GET: Posts/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Posts == null)
			{
				return NotFound();
			}

			var posts = await _context.Posts
				.FirstOrDefaultAsync(m => m.Id == id);
			if (posts == null)
			{
				return NotFound();
			}

			return View(posts);
		}

		// POST: Posts/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Posts == null)
			{
				return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
			}
			var posts = await _context.Posts.FindAsync(id);
			if (posts != null)
			{
				_context.Posts.Remove(posts);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool PostsExists(int id)
		{
			return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
