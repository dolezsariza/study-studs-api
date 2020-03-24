using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyStud.RequestModels
{
    public class FilePostRequest
    {
        public int TopicId { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
