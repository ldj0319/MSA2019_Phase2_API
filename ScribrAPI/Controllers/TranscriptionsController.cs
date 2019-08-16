using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScribrAPI.Model;

namespace ScribrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranscriptionsController : ControllerBase
    {
        private readonly scriberContext _context;

        public TranscriptionsController(scriberContext context)
        {
            _context = context;
        }
       
        private bool TranscriptionExists(int id)
        {
            return _context.Transcription.Any(e => e.TranscriptionId == id);
        }
    }
}
