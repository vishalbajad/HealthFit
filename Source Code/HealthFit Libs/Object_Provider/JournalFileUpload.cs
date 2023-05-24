using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthFit.Object_Provider.Model
{
    public class JournalFileUpload
    {
        public int JournalId { get; set; }
        public IFormFile CoverPhotofile { get; set; }
        public IFormFile JournalFile { get; set; }
    }
}
