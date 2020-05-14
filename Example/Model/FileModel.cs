using System;
using Microsoft.AspNetCore.Http;

namespace Example.Model
{
    public class FileModel
    {
        public string Name { get; set; }
        public IFormFile PDF { get; set; }
        public IFormFile XML { get; set; }


    }
}
