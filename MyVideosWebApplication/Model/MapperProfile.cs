using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MyVideosWebApplication.Model
{
  public class MapperProfile: Profile
  {
    public MapperProfile()
    {
      CreateMap<MyVideos, VideoDTO>();
      CreateMap<VideoDTO, MyVideos>();
    }
  }
}
