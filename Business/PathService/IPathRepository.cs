using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.PathService
{
    public interface IPathRepository
    {
        public Task<Graph> GetPath();

    }
}
