using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> UploadFile(FilePostRequest request)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await request.FormFile.CopyToAsync(memoryStream);
                    var file = new AppFile
                    {
                        OwnerId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value,
                        TopicId = request.TopicId,
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
    }
}