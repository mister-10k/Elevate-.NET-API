using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Shared
{
    public class StringToDateTimeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            DateTime dateTime;

            if (source == null)
            {
                return default(DateTime);
            }

            if (DateTime.TryParse(source, out dateTime))
            {
                return dateTime;
            }

            return default(DateTime);
        }
    }
}
