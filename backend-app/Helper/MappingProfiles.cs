using AutoMapper;
using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Device, DeviceDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<Sensor, SensorDTO>();
            CreateMap<SensorData, SensorDataDTO>();
            CreateMap<UserDevice, UserDeviceDTO>();

            CreateMap<DeviceDTO, Device>();
            CreateMap<UserDTO, User>();
            CreateMap<SensorDTO, Sensor>();
            CreateMap<SensorDataDTO, SensorData>();
            CreateMap<UserDeviceDTO, UserDevice>();
        }
    }
}