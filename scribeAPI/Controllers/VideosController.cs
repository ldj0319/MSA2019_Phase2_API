using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using scribeAPI.Helper;
using scribeAPI.Model;
using scribeAPI.DAL;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace scribeAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class VideosController : ControllerBase
  {
        public class URLDTO {
          public String URL { get; set; }
        }
        private readonly scriberContext _context;
        private IVideoRepository videoRepository;
        private readonly IMapper _mapper;

        public VideosController(scriberContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            this.videoRepository = new VideoRepository(new scriberContext());
        }

        // GET: api/Videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideo()
        {
            return await _context.Video.ToListAsync();
        }

        // GET: api/Videos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideo(int id)
        {
            var video = await _context.Video.FindAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            return video;
        }

        // PUT: api/Videos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideo(int id, Video video)
        {
            if (id != video.VideoId)
            {
                return BadRequest();
            }

            _context.Entry(video).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
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

        // POST: api/Videos
        [HttpPost]
        public async Task<ActionResult<Video>> PostVideo(URLDTO URL)
        {
            Video newVideo;
            String videoId;
            try
            {
              videoId = YouTubeHelper.GetVideoIdFromURL(URL.URL);
              newVideo = YouTubeHelper.GetVideoFromId(videoId);
            }
            catch {
              return BadRequest("Invalid YouTube URL");
            }
            _context.Video.Add(newVideo);
            await _context.SaveChangesAsync();

            TranscriptionsController transcriptionsController = new TranscriptionsController(new scriberContext());
            List<Transcription> transcriptions = YouTubeHelper.GetTranscriptions(videoId);

            Task addCaptions = Task.Run(async () =>
            {
              for (int i = 0; i < transcriptions.Count; i++)
              {
                // Get the transcription objects form transcriptions and assign VideoId to id, the primary key of the newly inserted video
                Transcription transcription = transcriptions.ElementAt(i);
                transcription.VideoId = newVideo.VideoId;
                // Add this transcription to the database
                await transcriptionsController.PostTranscription(transcription);
                Console.WriteLine("inserting transcription" + i);
              }
            });
            

           return CreatedAtAction("GetVideo", new { id = newVideo.VideoId }, newVideo);
        }

        // DELETE: api/Videos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Video>> DeleteVideo(int id)
        {
            var video = await _context.Video.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.Video.Remove(video);
            await _context.SaveChangesAsync();

            return video;
        }

        // GET api/Videos/SearchByTranscriptions/HelloWorld
        [HttpGet("SearchByTranscriptions/{searchString}")]
        public async Task<ActionResult<IEnumerable<Video>>> Search(string searchString)
        {
          if (String.IsNullOrEmpty(searchString))
          {
            return BadRequest("Search string cannot be null or empty.");
          }

          // Choose transcriptions that has the phrase 
          var videos = await _context.Video.Include(video => video.Transcription).Select(video => new Video
          {
            VideoId = video.VideoId,
            VideoTitle = video.VideoTitle,
            VideoLength = video.VideoLength,
            WebUrl = video.WebUrl,
            ThumbnailUrl = video.ThumbnailUrl,
            IsFavourite = video.IsFavourite,
            Transcription = video.Transcription.Where(tran => tran.Phrase.Contains(searchString)).ToList()
          }).ToListAsync();

          // Removes all videos with empty transcription
          videos.RemoveAll(video => video.Transcription.Count == 0);
          return Ok(videos);  
        }

      //PUT with PATCH to handle isFavourite
      [HttpPatch("update/{id}")]
      public VideoDTO Patch(int id, [FromBody]JsonPatchDocument<VideoDTO> videoPatch)
      {
        //get original video object from the database
        Video originVideo = videoRepository.GetVideoByID(id);
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


    private bool VideoExists(int id)
        {
            return _context.Video.Any(e => e.VideoId == id);
        }
    }
}
