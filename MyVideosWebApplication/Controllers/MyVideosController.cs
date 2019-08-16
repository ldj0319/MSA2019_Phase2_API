using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVideosWebApplication.Helper;
using MyVideosWebApplication.Model;
using Microsoft.AspNetCore.Mvc;
using MyVideosWebApplication.DAL;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace MyVideosWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyVideosController : ControllerBase
    {
        private readonly scriberContext _context;

        public class URLDTO
        {
          public String URL { get; set; }
        }
        private IVideoRepository videoRepository;
        private readonly IMapper _mapper;

    public MyVideosController(scriberContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            this.videoRepository = new VideoRepository(new scriberContext());
        }

        // GET: api/MyVideos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MyVideos>>> GetMyVideos()
        {
            return await _context.MyVideos.ToListAsync();
        }

        // GET: api/MyVideos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MyVideos>> GetMyVideos(int id)
        {
            var myVideos = await _context.MyVideos.FindAsync(id);

            if (myVideos == null)
            {
                return NotFound();
            }

            return myVideos;
        }

        // PUT: api/MyVideos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMyVideos(int id, MyVideos myVideos)
        {
            if (id != myVideos.VideoId)
            {
                return BadRequest();
            }

            _context.Entry(myVideos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MyVideosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MyVideos
        [HttpPost]
        public async Task<ActionResult<MyVideos>> PostMyVideos([FromBody]URLDTO data)
        {
          MyVideos video;
          String videoURL;
          String videoId;
          try
          {
            // Constructing the video object from our helper function
            videoURL = data.URL;
            videoId = YouTubeHelper.GetVideoIdFromURL(videoURL);
            video = YouTubeHelper.GetVideoInfo(videoId);
          }
          catch
          {
            return BadRequest("Invalid YouTube URL");
          }

          // Add this video object to the database
          _context.MyVideos.Add(video);
          await _context.SaveChangesAsync();

          // Return success code and the info on the video object
          return CreatedAtAction("GetVideo", new { id = video.VideoId }, video);
        }

        [HttpPatch("update/{id}")]
        public VideoDTO Patch(int id, [FromBody]JsonPatchDocument<VideoDTO> videoPatch)
        {
          //get original video object from the database
          MyVideos originVideo = videoRepository.GetVideoByID(id);
          //use automapper to map that to DTO object
          VideoDTO videoDTO = _mapper.Map<VideoDTO>(originVideo);
          //apply the patch to that DTO
          videoPatch.ApplyTo(videoDTO);
          //use automapper to map the DTO back ontop of the database object
          _mapper.Map(videoDTO, originVideo);
          //update video in the database
          _context.Update(originVideo);
          _context.SaveChanges();
          return videoDTO;
        }

        // DELETE: api/MyVideos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MyVideos>> DeleteMyVideos(int id)
        {
          List<int> videoIDlist = new List<int>(new int[] { 1 });
          if (videoIDlist.Contains(id))
          {
            return BadRequest("The video cannot be deleted");
          }

          var video = await _context.MyVideos.FindAsync(id);
          if (video == null)
          {
            return NotFound();
          }

          _context.MyVideos.Remove(video);
          await _context.SaveChangesAsync();

          return video;
        }

        private bool MyVideosExists(int id)
        {
            return _context.MyVideos.Any(e => e.VideoId == id);
        }
    }
}
