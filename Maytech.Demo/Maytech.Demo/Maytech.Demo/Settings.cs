using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maytech.Demo
{
    internal static class Settings
    {
        public const int DEFAULT_CHUNK_SIZE = 4 * 1000000;//MB
        public readonly static int MAX_UPLOAD_COUNT = System.Environment.ProcessorCount * 2;
        public readonly static string HOME_DIR = "Home";
    }
}
