using ScribrAPI.Controllers;
using ScribrAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ScribrAPI.Helper;
using System.Threading.Tasks;
using AutoMapper;
using ScribrAPI.DAL;
using Microsoft.AspNetCore.JsonPatch;

namespace ScribrAPIUnitTests
{
  [TestClass]
  class VideosControllerUnitTests
  {
    public static readonly DbContextOptions<scriberContext> options
    = new DbContextOptionsBuilder<scriberContext>()
    .UseInMemoryDatabase(databaseName: "testDatabase")
    .Options;
    public IMapper mapper;
    public IVideoRepository videoRepository;
    public static readonly IList<Video> video = new List<Video>
        {
            new Video()
            {
                VideoTitle = "MSA 2019 | Phase 2 | Training - Day 1",
                WebUrl = "https://www.youtube.com/watch?v=Z6U6g1wn8RE",
                VideoLength = 15189,
                IsFavourite = true,
                ThumbnailUrl = "https://i.ytimg.com/vi/Z6U6g1wn8RE/mqdefault.jpg"
            }
        };

    [TestInitialize]
    public void SetupDb()
    {
      using (var context = new scriberContext(options))
      {
        // populate the db
        context.Video.Add(video[0]);
        context.SaveChanges();
      }
    }

    [TestCleanup]
    public void ClearDb()
    {
      using (var context = new scriberContext(options))
      {
        // clear the db
        context.Video.RemoveRange(context.Video);
        context.SaveChanges();
      };
    }

    [TestMethod]
    public async Task TestGetSuccessfully()
    {
      using (var context = new scriberContext(options))
      {
        mapper = Mapper.Instance;
        VideosController videosController = new VideosController(context, mapper);
        ActionResult<IEnumerable<Video>> result = await videosController.GetVideo() as ActionResult<IEnumerable<Video>>;
        Assert.IsNotNull(result);
      }
    }


  }

}
