using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistConverter.Core
{
    public interface IConversionService
    {
        string Convert(string playlist);
    }
}
