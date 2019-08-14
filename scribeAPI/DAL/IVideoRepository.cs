using scribeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scribeAPI.DAL
{
  public interface IVideoRepository
  {
    IEnumerable<Video> GetVideos();
    Video GetVideoByID(int VideoId);
    void InsertVideo(Video video);
    void DeleteVideo(int VideoId);
    void UpdateVideo(Video video);
    void Save();
  }
}
