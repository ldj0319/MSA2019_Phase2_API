using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyVideosWebApplication.Model;

namespace MyVideosWebApplication.DAL
{
  public interface IVideoRepository : IDisposable
  {
    IEnumerable<MyVideos> GetVideos();
    MyVideos GetVideoByID(int VideoId);
    void InsertVideo(MyVideos video);
    void DeleteVideo(int VideoId);
    void UpdateVideo(MyVideos video);
    void Save();
  }
}
