using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using MyVideosWebApplication.Model;
using Newtonsoft.Json;

namespace MyVideosWebApplication.Helper
{
  public class YouTubeHelper
  {
    public static void testProgram()
    {
      // Console.WriteLine(GetVideoIdFromURL("https://www.youtube.com/watch?v=ehvz3iN8pp4"));

      // Pause the program execution
      // Console.ReadLine();

      GetVideoInfo("BZbChKzedEk");
    }

    public static String GetVideoIdFromURL(String videoURL)
    {
      // Extract the string after the '=' sign
      // e.g. https://www.youtube.com/watch?v=ehvz3iN8pp4 becomes ehvz3iN8pp4 
      int indexOfFirstId = videoURL.IndexOf("=") + 1;
      String videoId = videoURL.Substring(indexOfFirstId);
      return videoId;
    }

    public static MyVideos GetVideoInfo(String videoId)
    {
      String APIKey = "AIzaSyASwvZlbJL3FPAH2jmaavy3Wy1Jtk74i5o";
      String YouTubeAPIURL = "https://www.googleapis.com/youtube/v3/videos?id=" + videoId + "&key=" + APIKey + "&part=snippet,contentDetails";

      // Use an http client to grab the JSON string from the web.
      String videoInfoJSON = new WebClient().DownloadString(YouTubeAPIURL);

      // Using dynamic object helps us to more effciently extract infomation from a large JSON String.
      dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(videoInfoJSON);

      // Extract information from the dynamic object.
      String title = jsonObj["items"][0]["snippet"]["title"];
      String thumbnailURL = jsonObj["items"][0]["snippet"]["thumbnails"]["medium"]["url"];
      String durationString = jsonObj["items"][0]["contentDetails"]["duration"];
      String videoUrl = "https://www.youtube.com/watch?v=" + videoId;

      // duration is given in this format: PT4M17S, we need to use a simple parser to get the duration in seconds.
      TimeSpan videoDuration = XmlConvert.ToTimeSpan(durationString);
      int duration = (int)videoDuration.TotalSeconds;

      // Create a new Video Object from the model defined in the API.
      MyVideos video = new MyVideos
      {
        VideoTitle = title,
        WebUrl = videoUrl,
        VideoLength = duration,
        IsFavourite = false,
        ThumbnailUrl = thumbnailURL,
        Count = 0
      };

      return video;
    }
  }
}
