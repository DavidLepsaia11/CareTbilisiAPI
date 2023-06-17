using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Domain.Interfaces
{
    public interface IDatabaseSettings
    {
        public string StudentCollectionName { get; set; }

        public string DatabaseName { get; set; }

        public string ConnectionString { get; set; }
    }
}
