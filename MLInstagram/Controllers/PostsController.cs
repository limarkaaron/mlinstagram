using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLInstagram.Data;
using MLInstagram.Models;
using Amazon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Amazon.S3.Model;
using System.Diagnostics;

namespace MLInstagram.Controllers
{
	[Authorize]
	public class PostsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _config;
		private readonly IOptions<AWSUserInfo> _options;
		private readonly UserManager<MLInstagramUser> _userManager;
		public PostsController(ApplicationDbContext context, IConfiguration config, IOptions<AWSUserInfo> options, UserManager<MLInstagramUser> userManager)
		{
			_context = context;
			_config = config;
			_options = options;
			_userManager = userManager;
		}

		// GET: Posts
		public async Task<IActionResult> Index()
		{
			var context = _context.Posts.Where(post => post.UploaderId == _userManager.GetUserId(User));
			ViewBag.CurrentUser = _userManager.Users.FirstOrDefault(x => x.UserName == _userManager.GetUserName(User));
			ViewBag.CurrentDate = DateTime.Now;
			return View(await context.ToListAsync());
		}

		// GET: Explore
		public async Task<IActionResult> Explore()
		{
			return View(await _context.Posts.ToListAsync());
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
			ViewBag.Uploader = _userManager.Users.FirstOrDefault(user => user.Id == posts.UploaderId);
			ViewBag.CurrentDate = DateTime.Now;
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
		public async Task<IActionResult> Create(IFormFile file, string caption)
		{
			string objectUrl;
			int uniqueNum = 0;
			if (_context.Posts.Count() !=0)
			{
				uniqueNum = _context.Posts.OrderBy(x => x.Id).Last().Id;
			}

			//access key and secret key with S3 region
			using (var amazonS3client = new AmazonS3Client(_options.Value.AccessKey, _options.Value.SecretKey, RegionEndpoint.APSoutheast1))
			{
				using (var memorystream = new MemoryStream())
				{
					file.OpenReadStream().CopyTo(memorystream);
					var request = new TransferUtilityUploadRequest
					{
						InputStream = memorystream,
						Key = file.FileName + uniqueNum,
						BucketName = _options.Value.S3Bucket,
						ContentType = file.ContentType
					};
					var transferUtility = new TransferUtility(amazonS3client);
					await transferUtility.UploadAsync(request);
					objectUrl = $"https://{_options.Value.S3Bucket}.s3.{RegionEndpoint.APSoutheast1.SystemName}.amazonaws.com/" + file.FileName + uniqueNum;

				}
			}
			Post fileDetails = new Post();
			fileDetails.FileName = file.FileName;
			fileDetails.DatePosted = DateTime.Now;
			fileDetails.Caption = caption;
			fileDetails.UploaderId = _userManager.GetUserId(User);
			fileDetails.S3Url = objectUrl;

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
		public async Task<IActionResult> DeleteConfirmed(string Filename)
		{
			using (var amazonS3client = new AmazonS3Client(_options.Value.AccessKey, _options.Value.SecretKey, RegionEndpoint.APSoutheast1))
			{
				var transferUtility = new TransferUtility(amazonS3client);
				await transferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
				{
					Key = Filename,
					BucketName = _options.Value.S3Bucket,
				});

				Post fileDetails = new Post();
				fileDetails = _context.Posts.FirstOrDefault(x => x.FileName.ToLower() == Filename.ToLower());

				_context.Posts.Remove(fileDetails);
				_context.SaveChanges();

				ViewBag.Success = "File Deleted Successfully on S3 Bucket";
				return RedirectToAction(nameof(Index));
			}
		}

		private bool PostsExists(int id)
		{
			return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
