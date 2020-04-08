using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using StudyStud.Models;
using StudyStud.RequestModels;

namespace StudyStud.Controllers
{
    [Route("files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly StudyDbContext _context;
        private readonly string[] permittedExtensions = { ".txt", ".jpg", ".png", ".pdf" };

        public FileController(StudyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var formFile = Request.Form.Files[0];
            var filename = Request.Form.FirstOrDefault(k => k.Key == "FileName").Value;
            var ext = Path.GetExtension(filename).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return BadRequest("Invalid file extension");
            }

            try
            {
                
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);
                    var file = new AppFile
                    {
                        FileName = WebUtility.HtmlEncode(filename),
                        OwnerId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value,
                        TopicId = int.Parse(Request.Form.FirstOrDefault(k => k.Key == "TopicId").Value),
                        Content = memoryStream.ToArray()
                    };

                    _context.FileList.Add(file);
                    await _context.SaveChangesAsync();
                }
                return Created("File is uploaded", "");
            }
            catch (DbUpdateException)
            {
                return StatusCode(406);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var file = await _context.FileList.FirstOrDefaultAsync(f => f.Id == id);

            if (file == null)
                return NotFound();

            file.FileName = WebUtility.HtmlDecode(file.FileName);
            Stream stream = new MemoryStream(file.Content);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(file.FileName, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = file.FileName
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _context.FileList.FirstOrDefaultAsync(f => f.Id == id);

            if (file == null)
                return NotFound("The file is not in the database");

            if (file.OwnerId != User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value)
                return BadRequest("You don't have the rights to do this.");

            try
            {
                _context.FileList.Remove(file);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException)
            {
                return StatusCode(406);
            }

        }
    }
}