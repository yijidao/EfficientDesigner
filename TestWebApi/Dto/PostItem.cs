using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Dto
{
    public class PostItem
    {
        public string Author { get; set; }

        public string Title { get; set; }

        public int Vote { get; set; }

        public DateTime UpdateTime { get; set; }

        public string Content { get; set; }

        public PostItem()
        {
            
        }

        public PostItem(string author, string title, int vote, DateTime updateTime, string content)
        {
            Author = author;
            Title = title;
            Vote = vote;
            UpdateTime = updateTime;
            Content = content;
        }
    }
}
