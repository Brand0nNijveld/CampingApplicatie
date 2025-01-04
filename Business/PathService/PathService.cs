using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.PathService
{
    public class PathService
    {
        private IPathRepository repository;

        public PathService(IPathRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Graph> GetMainPath()
        {
            return await repository.GetPath();
        }
    }
}
