using AutoMapper;
using HRM.DTOs;
using HRM.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace HRM.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //Domain to Dto
            CreateMap<User, UserDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Currency, CurrencyDto>();
            CreateMap<Attendance, AttendanceDto>();

            //Dto to Domain
            CreateMap<UserDto, User>().ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<EmployeeDto, Employee>().ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<CurrencyDto, Currency>().ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<AttendanceDto, Attendance>().ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}