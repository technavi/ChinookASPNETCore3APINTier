using Chinook.Domain.Entities;
using Chinook.Domain.Supervisor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chinook.WEB.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IChinookSupervisor chinookSupervisor;
        public IEnumerable<Album> Albums { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IChinookSupervisor chinookSupervisor)
        {
            _logger = logger;
            this.chinookSupervisor = chinookSupervisor;
        }

        public void OnGet()
        {
            Albums = chinookSupervisor.GetAllAlbum();
        }
    }
}
