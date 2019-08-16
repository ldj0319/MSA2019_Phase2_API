using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyVideosWebApplication.Model;


namespace MyVideosWebApplication.DAL
{
  public class VideoRepository : IVideoRepository, IDisposable
  {
    private scriberContext context;

    public VideoRepository(scriberContext context)
    {
      this.context = context;
    }

    public IEnumerable<MyVideos> GetVideos()
    {
      return context.MyVideos.ToList();
    }

    public MyVideos GetVideoByID(int id)
    {
      return context.MyVideos.Find(id);
    }

    public void InsertVideo(MyVideos video)
    {
      context.MyVideos.Add(video);
    }

    public void DeleteVideo(int videoId)
    {
      MyVideos video = context.MyVideos.Find(videoId);
      context.MyVideos.Remove(video);
    }

    public void UpdateVideo(MyVideos video)
    {
      context.Entry(video).State = EntityState.Modified;
    }

    public void Save()
    {
      context.SaveChanges();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          context.Dispose();
        }
      }
      this.disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
