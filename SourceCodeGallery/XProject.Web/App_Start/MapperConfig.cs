using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NS;

using XProject.Domain.Helpers;


// ReSharper disable once CheckNamespace

namespace XProject.Web
{
    public static class MapperConfig
    {
        public static void RegisterMappers()
        {
            Mapper.CreateMap<Enumeration, int>().ConvertUsing<EnumerationTypeConverter>();

        }
    }
}
