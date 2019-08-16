using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MyVideosWebApplication.Model
{
    public partial class MyVideos
    {
        public MyVideos()
        {
            Comment = new HashSet<Comment>();
        }

        public int VideoId { get; set; }
        [Required]
        [StringLength(255)]
        public string VideoTitle { get; set; }
        public int VideoLength { get; set; }
        [Required]
        [Column("WebURL")]
        [StringLength(255)]
        public string WebUrl { get; set; }
        [Required]
        [Column("ThumbnailURL")]
        [StringLength(255)]
        public string ThumbnailUrl { get; set; }
        [Column("isFavourite")]
        public bool IsFavourite { get; set; }
        [Column("count")]
        public int Count { get; set; }

        [InverseProperty("Video")]
        public virtual ICollection<Comment> Comment { get; set; }
    }

    [DataContract]
    public class VideoDTO
    {
      [DataMember]
      public int VideoId { get; set; }

      [DataMember]
      public string VideoTitle { get; set; }

      [DataMember]
      public int VideoLength { get; set; }

      [DataMember]
      public string WebUrl { get; set; }

      [DataMember]
      public string ThumbnailUrl { get; set; }

      [DataMember]
      public bool IsFavourite { get; set; }

      [DataMember]
      public bool Count { get; set; }
    }


}
