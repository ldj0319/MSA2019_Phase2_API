using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVideosWebApplication.Model
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int? VideoId { get; set; }
        [Required]
        [StringLength(255)]
        public string Comments { get; set; }

        [ForeignKey("VideoId")]
        [InverseProperty("Comment")]
        public virtual MyVideos Video { get; set; }
    }
}
