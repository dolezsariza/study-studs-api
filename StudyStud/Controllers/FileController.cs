using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public FileController(StudyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                
                using (var memoryStream = new MemoryStream())
                {
                    await Request.Form.Files[0].CopyToAsync(memoryStream);
                    var file = new AppFile
                    {
                        FileName = Request.Form.FirstOrDefault(k => k.Key == "FileName").Value,
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
            Stream stream = new MemoryStream(file.Content);

            if(stream == null)
                return NotFound();

            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(file.FileName, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = file.FileName
            };
        }
    }
}